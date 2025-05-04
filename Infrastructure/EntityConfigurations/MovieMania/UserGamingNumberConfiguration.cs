using Domain.Entity.MovieMania;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.MovieMania
{
    public class UserGamingNumberConfiguration : IEntityTypeConfiguration<UserGamingNumber>
    {
        public void Configure(EntityTypeBuilder<UserGamingNumber> builder)
        {
            builder.HasIndex(x => x.UserId).IsUnique();
        }
    }
}
