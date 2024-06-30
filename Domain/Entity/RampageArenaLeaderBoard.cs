namespace Domain.Entity
{
    public class RampageArenaLeaderBoard : BaseEntity
    {
        public string PlayerName { get; set; }

        public long PlayerScore { get; set; }

        public int Rank { get; set; }
    }
}
