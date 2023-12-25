namespace GameLeaderBoard.DTOs
{
    public class GetScoreDto
    {
        public int Rank { get; set; }
        public string PlayerName { get; set; }
        public long Score { get; set; }
    }
}
