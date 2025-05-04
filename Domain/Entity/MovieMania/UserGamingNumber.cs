using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.MovieMania
{
    public class UserGamingNumber : BaseEntity
    {
        public string UserId { get; set; }

        public int TotalGamePlayed { get; set; }
    }
}
