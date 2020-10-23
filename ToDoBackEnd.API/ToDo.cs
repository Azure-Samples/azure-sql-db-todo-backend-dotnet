using System;

namespace ToDoBackEnd.API
{
    public class ToDo
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public bool Completed { get; set; }
    }
}
