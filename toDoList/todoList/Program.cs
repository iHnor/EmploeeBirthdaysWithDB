using System;
using System.Collections.Generic;
using Npgsql;
using System.Threading.Tasks;
namespace todoList
{
    class ConnectToBD
    {
        private List<TaskList> tasks = new List<TaskList>();

        public async Task<List<TaskList>> Read(NpgsqlConnection conn)
        {

            List<TaskList> tasks = new List<TaskList>();


            await using (var cmd = new NpgsqlCommand("Select \"Title\", \"Description\", \"DueDate\", \"Done\", \"List_Group\" from todo", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    tasks.Add(new TaskList(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2), reader.GetBoolean(3), reader.GetInt32(4)));

                }
            return tasks;
        }

        public async Task<bool> Create(NpgsqlConnection conn)
        {
            string Title = "Test title";
            string Desc = "Test Descript";
            DateTime DueDate = new DateTime();
            bool Done = true;
            int List_Group = 1;


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

        public async Task<bool> Update(NpgsqlConnection conn)
        {
            string Title = "Update title";
            string Desc = "";
            DateTime DueDate = DateTime.Now;
            bool Done = true;
            int List_Group = 2;
            int id = 2;

            await using (var cmd = new NpgsqlCommand("UPDATE todo SET \"Title\"=@Title, \"Description\"= @Description, \"DueDate\" =  @DueDate, \"Done\" =  @Done, \"List_Group\" =  @List_Group WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("Title", Title);
                cmd.Parameters.AddWithValue("Description", Desc);
                cmd.Parameters.AddWithValue("DueDate", DueDate);
                cmd.Parameters.AddWithValue("Done", Done);
                cmd.Parameters.AddWithValue("List_Group", List_Group);
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }

        public async Task<bool> Delete(NpgsqlConnection conn)
        {
            int delId = 5;
            await using (var cmd = new NpgsqlCommand("DELETE FROM todo WHERE \"id\"=@id;", conn))
            {
                cmd.Parameters.AddWithValue("id", delId);
                await cmd.ExecuteNonQueryAsync();
            }

            return true;
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

            // bool result = await connect.Create(conn);
            // bool result = await connect.Delete(conn);
            // bool result = await connect.Update(conn);

            

            // List<TaskList> taskLists = await connect.Read(conn);

            // Print(taskLists);
        }
        static void Print(List<TaskList> taskLists)
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
