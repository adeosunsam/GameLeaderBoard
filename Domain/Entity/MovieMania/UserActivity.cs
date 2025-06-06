namespace Domain.Entity.MovieMania
{
    public class UserActivity : BaseEntity
    {
        public string UserId { get; set; }
        public string? SenderId { get; set; }
        public ActivityEnum ActivityAction { get; set; }
        public string? TopicId { get; set; }
        public string? GroupId { get; set; }
    }

    public enum ActivityEnum
    {
        Challenge = 1,
        Follow
    }

    public enum ManageFriend
    {
        Accept = 1,
        Decline
    }
}
