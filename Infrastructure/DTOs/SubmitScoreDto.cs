using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs
{
    public class SubmitScoreDto
    {
        public string? PlayerId { get; set; }
        public string? PlayerName { get; set; }
        [Required]
        public long Score { get; set; }
    }
}
