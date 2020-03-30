using System.Collections.Generic;

namespace RTSTicket.Data.Models
{
	public class Role
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public IEnumerable<UserRolse> Users { get; set; } = new List<UserRolse>();
	}
}
