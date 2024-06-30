namespace Domain.Entity
{
    public class MovieManiaLeaderBoard : BaseEntity
    {
        public string PlayerId { get; set; }

        public string PlayerName { get; set; }

        public long PlayerScore { get; set; }

        public string MovieId { get; set; }
    }
}
