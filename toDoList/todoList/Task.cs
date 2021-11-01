using System;

namespace todoList
{
    public class Task
    {
        private string Title;
        private string Description;
        private DateTime DueDate;
        private bool Done;
        private int List_Group;

        public Task(string Title, string Description, DateTime DueDate, bool Done, int List_Group)
        {
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;
            this.Done = Done;
            this.List_Group = List_Group;
        }
        // public string getName()
        // {
        //     return nameOfPerson;
        // }
        // public DateTime getDateBirth()
        // {
        //     return birthday;
        // }
    }
}