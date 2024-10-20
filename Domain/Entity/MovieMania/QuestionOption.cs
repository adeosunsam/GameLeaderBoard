using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.MovieMania
{
    public class QuestionOption : BaseEntity
    {
        public string Title { get; set; }
        public bool IsCorrectOption { get; set; }

        [ForeignKey("Question")]
        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
