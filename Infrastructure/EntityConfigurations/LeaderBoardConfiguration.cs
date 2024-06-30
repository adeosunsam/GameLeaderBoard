using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

namespace Infrastructure.EntityConfigurations
{
    public class LeaderBoardConfiguration : IEntityTypeConfiguration<RampageArenaLeaderBoard>
    {
        public void Configure(EntityTypeBuilder<RampageArenaLeaderBoard> builder)
        {
            builder.HasIndex(x => x.PlayerName).IsUnique();
        }
    }
}
