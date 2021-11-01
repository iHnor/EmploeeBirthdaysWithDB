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
        public GetTitle(){return Title;}
        public GetDesc(){return Description;}
        public GetDueDate(){return DueDate;}
        public GetDone(){return Done;}
        public GetListGroup(){return List_Group;}
    }
}