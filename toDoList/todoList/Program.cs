using System;

namespace todoList
{
    // class Task
    // {

    // }
    class ConnectToBD
    {
        private List<Employee> tasks = new List<Employee>();

        private async Task ReadEmployees()
        {
            var connString = "Host=127.0.0.1;Username=admin;Password=12341234qs;Database=todolist";


            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            await using (var cmd = new NpgsqlCommand("Select name, date from Employee_Birthday", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    employees.Add(new Employee(reader.GetString(0), reader.GetDateTime(1)));
                }
        }

        private async List<Employee> getList()
        {
            Task read = ReadEmployees();
            read.Wait();
        }

        //Create

        //Read
        public List<Employee> Read()
        {
            
            return employees;
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
