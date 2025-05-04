namespace Infrastructure.DTOs
{
    public struct TokenValidation
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
    }
}
