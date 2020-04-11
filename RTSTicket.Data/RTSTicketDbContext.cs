
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

			modelBuilder.Entity<UserRolse>()
			.HasOne(r=>r.User)
			.WithMany(r=>r.Rolses)
			.HasForeignKey(u => u.UserId);

			modelBuilder.Entity<UserRolse>()
				.HasOne(r=>r.Role)
				.WithMany(r=>r.Users)
				.HasForeignKey(r => r.RoleId);

			base.OnModelCreating(modelBuilder);
		}
	}
}
