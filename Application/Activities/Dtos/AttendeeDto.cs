namespace Application.Activities.Dtos
{
    public class AttendeeDto
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool IsHost { get; set; }
        public bool Following { get; set; }
        public int FollowingsCount { get; set; }
        public int FollowersCount { get; set; }
    }
}