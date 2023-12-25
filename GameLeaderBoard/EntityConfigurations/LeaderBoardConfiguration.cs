using GameLeaderBoard.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Merchant_Lending.Infrastructure.EntityConfigurations
{
    public class LeaderBoardConfiguration : IEntityTypeConfiguration<LeaderBoard>
    {
        public void Configure(EntityTypeBuilder<LeaderBoard> builder)
        {
            builder.HasIndex(x => x.PlayerName).IsUnique();
        }
    }
}
