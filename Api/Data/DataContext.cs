using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Api.Entities;
namespace Api.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext()
        {
            Database.EnsureCreated();
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Book> Books => Set<Book>();

        public DbSet<Order> Orders => Set<Order>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>().HasOne(x => x.Order);

            builder.Entity<Order>().HasMany(order => order.Books).WithMany(x => x.Orders);
        }

    }
}
