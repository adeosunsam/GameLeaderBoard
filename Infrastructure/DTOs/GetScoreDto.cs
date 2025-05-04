namespace Infrastructure.DTOs
{
    public class GetScoreDto
    {
        public string PlayerId { get; set; }
        public int Rank { get; set; }
        public string PlayerName { get; set; }
        public long Score { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
