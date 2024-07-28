using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.MovieMania
{
    public class FollowedTopic : BaseEntity
    {
        //[ForeignKey("AppUser")]
        public string UserId { get; set; }

        [ForeignKey("Topic")]
        public string TopicId { get; set; }

        //public virtual AppUser AppUser { get; set; }

        public virtual Topic Topic { get; set; }
    }
}
