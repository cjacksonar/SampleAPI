using Classes;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class APIDbContext : DbContext
    {
        public APIDbContext()
        {
        }

        public APIDbContext(DbContextOptions<APIDbContext> options)
           : base(options)
        {

        }
        public DbSet<Data.Entities.Fund> Funds { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }        
        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<DTOs.vwContributions> vwContributions { get; set; }
        public DbSet<DTOs.ViewContributionsList> ViewContributionsList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Globals.ApplicationDatabaseConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Data.DTOs.vwContributions>()
               .HasNoKey();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Data.DTOs.ViewContributionsList>()
               .HasNoKey();
        }
    }
}
