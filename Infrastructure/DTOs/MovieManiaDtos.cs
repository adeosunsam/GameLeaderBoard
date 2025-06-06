using System.ComponentModel.DataAnnotations;
using Domain.Entity.MovieMania;

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

        public class UserFollowRequest
        {
            [Required]
            public string UserId { get; set; }
            [Required]
            public string FriendId { get; set; }
        }

        public class ManageFriendRequest
        {
            [Required]
            public string UserId { get; set; }
            [Required]
            public string FriendId { get; set; }
            [Required]
            public ManageFriend Action { get; set; }
        }

        public class UserActivityResponse
        {
            public string Id { get; set; }
            public string SenderName { get; set; }
            public string SenderId { get; set; }
            public string? UserImage { get; set; }
            public ActivityEnum Activity { get; set; }
            public string? TopicName { get; set; }
            public string? TopicId { get; set; }
            public string? GroupId { get; set; }
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

        public class UserDetailDto
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

        public class RelayServiceCallback
        {
            public Transaction transaction { get; set; }
            public Networkmap networkMap { get; set; }
            public Report report { get; set; }

            public class Transaction
            {
                public string TxTp { get; set; }
                public Fitofipmtsts FIToFIPmtSts { get; set; }
            }

            public class Fitofipmtsts
            {
                public Grphdr GrpHdr { get; set; }
                public Txinfandsts TxInfAndSts { get; set; }
            }

            public class Grphdr
            {
                public string MsgId { get; set; }
                public DateTime CreDtTm { get; set; }
            }

            public class Txinfandsts
            {
                public string OrgnlInstrId { get; set; }
                public string OrgnlEndToEndId { get; set; }
                public string TxSts { get; set; }
                public DateTime AccptncDtTm { get; set; }
                public Instgagt InstgAgt { get; set; }
                public Instdagt InstdAgt { get; set; }
            }

            public class Instgagt
            {
                public Fininstnid FinInstnId { get; set; }
            }

            public class Fininstnid
            {
                public Clrsysmmbid ClrSysMmbId { get; set; }
            }

            public class Clrsysmmbid
            {
                public string MmbId { get; set; }
            }

            public class Instdagt
            {
                public Fininstnid1 FinInstnId { get; set; }
            }

            public class Fininstnid1
            {
                public Clrsysmmbid1 ClrSysMmbId { get; set; }
            }

            public class Clrsysmmbid1
            {
                public string MmbId { get; set; }
            }

            public class Networkmap
            {
                public bool active { get; set; }
                public string cfg { get; set; }
                public Message[] messages { get; set; }
            }

            public class Message
            {
                public string id { get; set; }
                public string cfg { get; set; }
                public string txTp { get; set; }
                public Typology[] typologies { get; set; }
            }

            public class Typology
            {
                public string id { get; set; }
                public string cfg { get; set; }
                public Rule[] rules { get; set; }
            }

            public class Rule
            {
                public string id { get; set; }
                public string cfg { get; set; }
            }

            public class Report
            {
                public string evaluationID { get; set; }
                public Metadata metaData { get; set; }
                public string status { get; set; }
                public DateTime timestamp { get; set; }
                public Tadpresult tadpResult { get; set; }
            }

            public class Metadata
            {
                public int prcgTmDP { get; set; }
                public int prcgTmED { get; set; }
            }

            public class Tadpresult
            {
                public string id { get; set; }
                public string cfg { get; set; }
                public Typologyresult[] typologyResult { get; set; }
                public int prcgTm { get; set; }
            }

            public class Typologyresult
            {
                public string id { get; set; }
                public string cfg { get; set; }
                public int result { get; set; }
                public Ruleresult[] ruleResults { get; set; }
                public int prcgTm { get; set; }
                public bool review { get; set; }
                public Workflow workflow { get; set; }
            }

            public class Workflow
            {
                public int alertThreshold { get; set; }
                public int interdictionThreshold { get; set; }
            }

            public class Ruleresult
            {
                public string id { get; set; }
                public string cfg { get; set; }
                public string subRuleRef { get; set; }
                public int prcgTm { get; set; }
                public int wght { get; set; }
            }
        }
    }
}
