using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.MovieMania
{
    public class Question : BaseEntity
    {
        public string Title { get; set; }

        public string? Image { get; set; }

        public int IndexNumber { get; set; }

        [ForeignKey("Topic")]
        public string TopicId { get; set; }

        public virtual Topic Topic { get; set; }
    }
}
