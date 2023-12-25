namespace GameLeaderBoard.Model
{
    public class LeaderBoard : BaseEntity
    {
        public string PlayerName { get; set; }

        public long PlayerScore { get; set; }

        public int Rank { get; set; }
    }
}
