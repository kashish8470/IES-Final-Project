namespace assignment5.Models
{
    public class UsersInfo
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Source { get; set; }

        public bool IsApproved { get; set; }
    }
}
