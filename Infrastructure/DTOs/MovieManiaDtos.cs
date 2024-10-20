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

        public class UserDetailResponseDto
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public string Image { get; set; }//image in base64
        }

        public class UserGamingCountDto
        {
            public string Id { get; set; }
            public int TotalGamePlayed { get; set; }
            public int TotalFriends { get; set; }
        }

        public class UserDetailRequestDto
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public string Image { get; set; }//image in base64
        }

        public class TopicResponseDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string Image { get; set; } // in base64
            public int QuestionCount { get; set; }
            public int FollowersCount { get; set; }
            public bool IsFollowed { get; set; }
        }

        public class QuestionDto
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string? Image { get; set; }
            public int IndexNumber { get; set; }
            public List<QuestionOption> Options { get; set; }
        }

        public class QuestionOption
        {
            public string Title { get; set; }
            public bool IsCorrectOption { get; set; }
        }
    }
}
