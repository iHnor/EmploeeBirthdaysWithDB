using System;

namespace todoList
{
    public class TaskList
    {
        private string Title;
        private string Description;
        private DateTime DueDate;
        private bool Done;
        private int List_Group;

        public TaskList(string Title, string Description, DateTime DueDate, bool Done, int List_Group)
        {
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;
            this.Done = Done;
            this.List_Group = List_Group;
        }
        public string GetTitle(){return Title;}
        public string GetDesc(){return Description;}
        public DateTime GetDueDate(){return DueDate;}
        public bool GetDone(){return Done;}
        public int GetListGroup(){return List_Group;}

    }
}