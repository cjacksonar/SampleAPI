namespace Data.DTOs
{
    public class Contributor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Comments { get; set; }
    }
}