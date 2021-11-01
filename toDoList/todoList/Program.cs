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
        // private List<TaskList> tasks = new List<TaskList>();

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

        //Create
        public async Task<List<TaskList>> Create(NpgsqlConnection conn)
        {
            return tasks;
        }

        //Update
        public async Task<List<TaskList>> Update(NpgsqlConnection conn)
        {
        
            return tasks;
        }


        //Delete
        public async Task<List<TaskList>> Delete(NpgsqlConnection conn)
        {
        
            return tasks;
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
            List<TaskList> taskLists = await connect.Read(conn);
        }
    }
}
