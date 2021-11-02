using System;
using System.Collections.Generic;
using Npgsql;
using System.Threading.Tasks;
namespace todoList
{
    // class Task
    // {

    // }
    class ConnectToBD
    {
        private List<TaskList> tasks = new List<TaskList>();

        //Read
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

        // Create
        public async Task Create(NpgsqlConnection conn)
        {
            string Title = "Test title";
            string Desc = "Test Descript";
            DateTime DueDate = new DateTime();
            bool Done = true;
            int List_Group = 1;


            await using (var cmd = new NpgsqlCommand("INSERT INTO todo (\"Title\", \"Description\", \"DueDate\", \"Done\", \"List_Group\") VALUES (@Title, @Description, @DueDate, @Done, @List_Group)", conn))
            {
                cmd.Parameters.AddWithValue("Title", Title);
                cmd.Parameters.AddWithValue("Description",Desc);
                cmd.Parameters.AddWithValue("DueDate", DueDate);
                cmd.Parameters.AddWithValue("Done", Done);
                cmd.Parameters.AddWithValue("List_Group", List_Group);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // //Update
        // public async Task<List<TaskList>> Update(NpgsqlConnection conn)
        // {

        //     return tasks;
        // }


        // //Delete
        // public async Task<List<TaskList>> Delete(NpgsqlConnection conn)
        // {

        //     return tasks;
        // }

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

            List<TaskList> taskLists = await connect.Read(conn);
            // Task CreateTask = connect.Create(conn);
            // CreateTask.Wait();

            Print(taskLists);
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
