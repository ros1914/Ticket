
namespace RTSTicket.Data
{
	using Microsoft.EntityFrameworkCore;
	using RTSTicket.Data.Models;

	public class RTSTicketDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<Role> Roles { get; set; }

		public DbSet<UserRolse> UserRolses { get; set; }

		public RTSTicketDbContext(DbContextOptions<RTSTicketDbContext> dbContext) : base(dbContext)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserRolse>()
				.HasKey(ur => new { ur.UserId, ur.RoleId });

			modelBuilder.Entity<User>()
			.HasMany(u => u.Rolses)
			.WithOne(u => u.User)
			.HasForeignKey(u => u.UserId);

			modelBuilder.Entity<Role>()
				.HasMany(r => r.Users)
				.WithOne(r => r.Role)
				.HasForeignKey(r => r.RoleId);

			base.OnModelCreating(modelBuilder);
		}
	}
}
