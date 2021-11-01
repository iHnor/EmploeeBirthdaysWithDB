using System;
using Npgsql;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeesSorter
{
    public class Connect
    {
        // static void Main()
        // {
        //     List<string> list = new List<string>();
        //     TMP(list);
        //     // System.Console.WriteLine(list[0]);
        // }
        public void TMP(List<string> list){
            Task tmp = Read(list);
            tmp.Wait();
        }
        static async Task Read(List<string> list)
        {
            var connString = "Host=127.0.0.1;Username=admin;Password=12341234qs;Database=todolist";


            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            await using (var cmd = new NpgsqlCommand("Select name, date from Employee_Birthday", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    list.Add(reader.GetString(0));
                    Console.WriteLine($"{reader.GetString(0)}: {reader.GetDate(1)}");
                }
        }
    }
}
