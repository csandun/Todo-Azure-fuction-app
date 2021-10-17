using System;

namespace CSandun.Todo
{
    public class Todo
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}