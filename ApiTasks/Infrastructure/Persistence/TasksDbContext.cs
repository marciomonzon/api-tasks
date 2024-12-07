using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TasksDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<ListCard> ListCards { get; set; }
        public DbSet<Card> Cards { get; set; }

        public TasksDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
