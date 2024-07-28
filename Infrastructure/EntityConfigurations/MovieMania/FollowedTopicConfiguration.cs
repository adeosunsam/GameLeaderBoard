using Domain.Entity.MovieMania;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.MovieMania
{
    public class FollowedTopicConfiguration : IEntityTypeConfiguration<FollowedTopic>
    {
        public void Configure(EntityTypeBuilder<FollowedTopic> builder)
        {
            builder.HasIndex(x => x.UserId);
        }
    }
}
