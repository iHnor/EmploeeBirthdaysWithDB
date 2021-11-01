using System;
using System.Collections.Generic;
using System.Globalization;
using Npgsql;
using System.Threading.Tasks;

namespace EmployeeBirthdays
{
    class ConnectToBD
    {
        private List<Employee> employees = new List<Employee>();

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

        public List<Employee> getListOfEmployees()
        {
            Task read = ReadEmployees();
            read.Wait();
            return employees;
        }
    }

    public class Employee
    {
        private string nameOfPerson;
        private DateTime birthday;
        public Employee(string nameOfPerson, DateTime birthday)
        {
            this.nameOfPerson = nameOfPerson;
            this.birthday = birthday;
        }
        public string getName()
        {
            return nameOfPerson;
        }
        public DateTime getDateBirth()
        {
            return birthday;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int planingHorizon = 1;
            ConnectToBD connect = new ConnectToBD();
            List<Employee> listOfEmployee = new List<Employee>(connect.getListOfEmployees());

            EmployeesSorter sorter = new EmployeesSorter(listOfEmployee);
            PrintBirthday(sorter, planingHorizon);
        }
        static void PrintBirthday(EmployeesSorter sorter, int plan)
        {
            DateTime today = DateTime.Now;

            for (int j = 0; j <= plan; j++)
            {
                if (sorter.isElemInDictionary(today.Month))
                {
                    string month = today.ToString("MMMM", CultureInfo.GetCultureInfo("ru-RU"));
                    System.Console.WriteLine($"{month} {today.Year}");
                    foreach (Employee value in sorter.GetSortEmployees(today.Month))
                    {
                        string day = value.getDateBirth().ToString("dd");
                        string name = value.getName();
                        DateTime age = value.getDateBirth();
                        Console.WriteLine($"({day}) - {name} ({getAge(age, today)})");
                    }
                }

                today = today.AddMonths(1);
            }
        }
        static string getAge(DateTime date, DateTime totalDate)
        {
            int age = totalDate.Subtract(date).Days / 365;

            return pluralization(age + 1);
        }
        static string pluralization(int numberOfAge)
        {
            string[] words = { "лет", "год", "года" };

            int sNumb = numberOfAge % 10;
            int dNumb = numberOfAge % 100;

            if ((10 <= dNumb && dNumb <= 20) || (sNumb == 0) || (5 <= sNumb && sNumb <= 20))
                return $"{numberOfAge} {words[0]}";
            else if (sNumb == 1)
                return $"{numberOfAge} {words[1]}";
            else
                return $"{numberOfAge} {words[2]}";
        }
    }
}