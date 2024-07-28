using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.MovieMania
{
    public class UserFriend : BaseEntity
    {
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }

        [Required]
        public string FriendId { get; set; }
    }
}
