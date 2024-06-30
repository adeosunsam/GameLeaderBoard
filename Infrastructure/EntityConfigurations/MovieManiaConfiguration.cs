using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

namespace Infrastructure.EntityConfigurations
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
