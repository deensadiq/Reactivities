using System;

namespace Application.Profiles
{
    public class ProfileParams
    {
        public bool FutureEvents { get; set; }
        public bool PastEvents { get; set; }
        public bool IsHost { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}