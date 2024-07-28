using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.MovieMania
{
    public class UserGamingNumber : BaseEntity
    {
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }

        public int TotalGamePlayed { get; set; }
    }
}
