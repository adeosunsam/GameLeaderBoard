namespace Infrastructure.DTOs
{
    public static class MovieManiaDtos
    {
        public class UserChallengeData
        {
            public string OpponentId { get; set; }
            /// <summary>
            /// playerId
            /// </summary>
            public string Id { get; set; }
            public string MovieId { get; set; }
            public string GameId { get; set; }
            /// <summary>
            /// player score
            /// </summary>
            public int Score { get; set; }
        }

        public class SaveScoreForLeaderBoardDto
        {
            public string Id { get; set; }
            public string PlayerName { get; set; }
            public string MovieId { get; set; }
            public int Score { get; set; }

            /// <summary>
            /// This is needed to delete the record from the cache
            /// </summary>
            public string GameId { get; set; }
        }
    }
}
