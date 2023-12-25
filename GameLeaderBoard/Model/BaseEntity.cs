using System.ComponentModel.DataAnnotations;

namespace GameLeaderBoard.Model
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
