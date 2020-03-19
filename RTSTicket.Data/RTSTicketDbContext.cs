
namespace RTSTicket.Data
{
	using Microsoft.EntityFrameworkCore;
	using RTSTicket.Data.Models;

	public class RTSTicketDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public RTSTicketDbContext(DbContextOptions<RTSTicketDbContext> dbContext) : base(dbContext)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
				

			base.OnModelCreating(modelBuilder);
		}
	}
}
