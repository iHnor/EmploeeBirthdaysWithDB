using System;
using System.Collections.Generic;
using Npgsql;
using System.Threading.Tasks;

namespace todoList
{
    class ConnectToBD
    {
        // private List<TodoItem> tasks = new List<TodoItem>();

        public async Task<List<TodoItem>> ReadLine(NpgsqlConnection conn)
        {

            List<TodoItem> tasks = new List<TodoItem>();


            await using (var cmd = new NpgsqlCommand("Select \"Title\", \"Description\", \"DueDate\", \"Done\", \"List_Group\" from todo", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    tasks.Add(new TodoItem(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2), reader.GetBoolean(3), reader.GetInt32(4)));

                }
            return tasks;
        }

        public async Task<bool> Create(NpgsqlConnection conn, string Title, string Desc, DateTime DueDate, bool Done, int List_Group)
        {
            try
            {
                await using (var cmd = new NpgsqlCommand("INSERT INTO todo (\"Title\", \"Description\", \"DueDate\", \"Done\", \"List_Group\") VALUES (@Title, @Description, @DueDate, @Done, @List_Group)", conn))
                {
                    cmd.Parameters.AddWithValue("Title", Title);
                    cmd.Parameters.AddWithValue("Description", Desc);
                    cmd.Parameters.AddWithValue("DueDate", DueDate);
                    cmd.Parameters.AddWithValue("Done", Done);
                    cmd.Parameters.AddWithValue("List_Group", List_Group);
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Update(NpgsqlConnection conn, int id, string updateColumn, string inputData)
        {
            if (updateColumn == "t")
            {
                string data = inputData;
                updateColumn = "Title";
            }
            else if (updateColumn == "dc")
            {
                string data = inputData;
                updateColumn = "Description";
            }

            else if (updateColumn == "dd")
            {
                DateTime data = Convert.ToDateTime(inputData);
                updateColumn = "DueDate";
            }

            else if (updateColumn == "d")
            {
                bool data = Convert.ToBoolean(inputData);
                updateColumn = "Done";
            }

            else if (updateColumn == "ld")
            {
                int data = Convert.ToInt32(inputData);
                updateColumn = "List_Group";
            }
            try
            {
                await using (var cmd = new NpgsqlCommand("UPDATE todo SET" + updateColumn + " = @data WHERE id=@id", conn))
                {
                    // cmd.Parameters.AddWithValue(updateColumn, data);
                    cmd.Parameters.AddWithValue("id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }


        }

        public async Task<bool> Delete(NpgsqlConnection conn, int idDelete)
        {
            try
            {
                await using (var cmd = new NpgsqlCommand("DELETE FROM todo WHERE \"id\"=@id;", conn))
                {
                    cmd.Parameters.AddWithValue("id", idDelete);
                    await cmd.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }
        static async Task MainAsync()
        {
            var connString = "Host=127.0.0.1;Username=admin;Password=12341234qs;Database=todolist";
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            ConnectToBD connect = new ConnectToBD();

            char CRUD;
            string Title;
            string Desc;
            DateTime DueDate;
            bool Done;
            int List_Group;
            int id;
            while (true)
            {
                Console.WriteLine("What do you want to do?\n C - Create; R - Read; U - Update; D - Delete; E - Exit\n");
                CRUD = Convert.ToChar(Console.ReadLine());
                if (CRUD == 'R')
                {
                    Print(await connect.ReadLine(conn));
                }
                else if (CRUD == 'C')
                {
                    Console.WriteLine("Title:");
                    Title = Console.ReadLine();
                    Console.WriteLine("Description:");
                    Desc = Console.ReadLine();
                    Console.WriteLine("DueDate:");
                    DueDate = Convert.ToDateTime(Console.ReadLine());
                    Console.WriteLine("Done:");
                    Done = Convert.ToBoolean(Console.ReadLine());
                    Console.WriteLine("List_Group:");
                    List_Group = Convert.ToInt32(Console.ReadLine());

                    if (await connect.Create(conn, Title, Desc, DueDate, Done, List_Group))
                        Console.WriteLine("Creation is successful\n");
                    else
                        Console.WriteLine("Creation is failed. Try again!\n");
                }
                else if (CRUD == 'U')
                {
                    Console.WriteLine("id:");
                    id = Console.Read();
                    Console.WriteLine("Update: t - Title; dc - Description; dd - DueDate; d - Done; ld - List_Group;");
                    string typeOfUpdate = Console.ReadLine();
                    Console.WriteLine("Value:");
                    string data = Console.ReadLine();

                    if (await connect.Update(conn, id, typeOfUpdate, data))
                        Console.WriteLine("Update is successful\n");
                    else
                        Console.WriteLine("Update is failed. Try again!\n");
                }
                else if (CRUD == 'D')
                {
                    Console.WriteLine("id:");
                    int idDelete = Convert.ToInt32(Console.ReadLine());
                    if (await connect.Delete(conn, idDelete))
                        Console.WriteLine("Delete is successful\n");
                    else
                        Console.WriteLine("Delete is failed. Try again!\n");
                }
            }
        }
        static void Print(List<TodoItem> taskLists)
        {
            for (int i = 0; i < taskLists.Count; i++)
            {
                var Title = taskLists[i].GetTitle();
                var Description = taskLists[i].GetDesc();
                var DueDate = taskLists[i].GetDueDate();
                var Done = taskLists[i].GetDone();
                var List_Group = taskLists[i].GetListGroup();
                char doneFlag = Done ? 'x' : ' ';

                System.Console.WriteLine($"[{doneFlag}] - {Title} ({DueDate.Year})\n{Description}\nList group - {List_Group}\n");
                System.Console.WriteLine();
            }
        }
    }
}
