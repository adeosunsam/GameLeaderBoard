using System.ComponentModel;

namespace Domain.Entity.MovieMania
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public string Image { get; set; } // in base64
        public int QuestionCount { get; set; }
        public int FollowersCount { get; set; }
    }

    public enum Category
    {
        Generals,
        Education,
        Tv,
        Movies,
        [Description("Naija Tech")]
        NaijaTech
    }
}
