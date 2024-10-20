namespace Domain.Entity.MovieMania
{
    public class AppUser : BaseEntity
    {
        public string? FirstName { get; set; }
        public string UserId { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; }//image in base64
    }
}
