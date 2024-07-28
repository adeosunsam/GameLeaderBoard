using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity.MovieMania;

namespace Infrastructure.EntityConfigurations.MovieMania
{
    public class MovieManiaConfiguration : IEntityTypeConfiguration<MovieManiaLeaderBoard>
    {
        public void Configure(EntityTypeBuilder<MovieManiaLeaderBoard> builder)
        {
            builder.HasIndex(x => x.PlayerId);
            builder.HasIndex(x => x.MovieId);
        }
    }
}
