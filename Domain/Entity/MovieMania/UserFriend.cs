using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.MovieMania
{
    public class UserFriend : BaseEntity
    {
        public string UserId { get; set; }

        [Required]
        public string FriendId { get; set; }
    }
}
