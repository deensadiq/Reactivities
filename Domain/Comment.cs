using System;

namespace Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public Activity Activity { get; set; }
        public AppUser Author { get; set; }
        public DateTime DateCreated { get; set; }
    }
}