using GameLeaderBoard.Model;
using Microsoft.EntityFrameworkCore;

namespace GameLeaderBoard.Context
{
    public class LeaderBoardContext : DbContext
    {
        public LeaderBoardContext(DbContextOptions<LeaderBoardContext> option) : base(option) { }

        public DbSet<LeaderBoard> LeaderBoards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(LeaderBoardContext).Assembly);

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries<BaseEntity>())
            {
                switch (item.State)
                {
                    case EntityState.Modified:
                        item.Entity.ModifiedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        item.Entity.CreatedOn = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
