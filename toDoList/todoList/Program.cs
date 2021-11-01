using System;
using System.Collections.Generic;
using Npgsql;

namespace todoList
{
    // class Task
    // {

    // }
    class ConnectToBD
    {
        private List<TaskList> tasks = new List<TaskList>();

        private async Task ReadTaskList()
        {

            var connString = "Host=127.0.0.1;Username=admin;Password=12341234qs;Database=todolist";
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            await using (var cmd = new NpgsqlCommand("Select name, date from Employee_Birthday", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    tasks.Add(new TaskList());
                }
        }

        private async void getList()
        {
            Task read = ReadTaskList();
            read.Wait();
        }

        //Create

        //Read
        public void Read()
        {
        }

        //Update

        //Delete
    }
    class Program
    {
        static void Main(string[] args)
        {
            ConnectToBD connect = new ConnectToBD();

        }
    }
}
