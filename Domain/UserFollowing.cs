namespace Domain
{
    public class UserFollowing
    {
        public string TargetId { get; set; }
        public AppUser Target { get; set; }
        public string ObserverId { get; set; }
        public AppUser Observer { get; set; }
    }
}