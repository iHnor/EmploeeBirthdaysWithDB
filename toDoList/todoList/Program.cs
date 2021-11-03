using System;
using System.Collections.Generic;
using Npgsql;
using System.Threading.Tasks;

namespace todoList
{
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
                Console.WriteLine("What do you want to do?\n C - Create; R - Read; U - Update; D - Delete;\n");
                CRUD = Convert.ToChar(Console.ReadLine());
                if (CRUD == 'R')
                {
                    Print(await connect.ReadDB(conn));
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
                    string chose;
                    Console.WriteLine("id:");
                    id = Convert.ToInt32(Console.ReadLine());
 
                    TaskColumn updateTask = new TaskColumn();
                    List<TodoItem> todoItems = await connect.ReadDB(conn);

                    Console.WriteLine("Update Title: (Y/N)");
                    chose = Console.ReadLine();
                    updateTask.Title = chose == "N" ? todoItems[id].GetTitle() : Console.ReadLine();

                    Console.WriteLine("Update Description: (Y/N)");
                    chose = Console.ReadLine();
                    updateTask.Desc = chose == "N" ? todoItems[id].GetDesc() : Console.ReadLine();
                    
                    Console.WriteLine("Update DueDate: (Y/N)");
                    chose = Console.ReadLine();
                    updateTask.DueDate = chose == "N" ? todoItems[id].GetDueDate() : Convert.ToDateTime(Console.ReadLine());

                    Console.WriteLine("Update Done: (Y/N)");
                    chose = Console.ReadLine();
                    updateTask.Done = chose == "N" ? todoItems[id].GetDone() : Convert.ToBoolean(Console.ReadLine());

                    Console.WriteLine("Update List_Group: (Y/N)");
                    chose = Console.ReadLine();
                    updateTask.List_Group = chose == "N" ? todoItems[id].GetListGroup() : Convert.ToInt32(Console.ReadLine());

                    if (await connect.Update(conn, id, updateTask))
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
