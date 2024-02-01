namespace Data.DTOs
{
    public class RegisteredUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int UserRoleId { get; set; }
        public int AllowEditing { get; set; }
        public int NumberOfLogins { get; set; }
        public DateTime LastLogin { get; set; }

    }
}