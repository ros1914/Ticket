using RTSTicket.Data;
using RTSTicket.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSTicket.Service.Implementation
{
	public class AdminService : IAdminService
	{
		private readonly RTSTicketDbContext dbContext;

		public AdminService(RTSTicketDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<bool> AddTorole(User user, string roleName)
		{
			if (user== null|| String.IsNullOrEmpty(roleName))
			{
				return false;
			}

			var users = await this.dbContext.Users.FindAsync(user.Id);

			var roles =  this.dbContext.Roles.Where(r=>r.Name==roleName).FirstOrDefault();

			if (user == null || roles==null)
			{
				return false;
			}

			var isHaveRole = this.dbContext.UserRolses.Where(r=>r.UserId==users.Id).Select(r=>r.RoleId==roles.Id).FirstOrDefault();
			if (!isHaveRole)
			{
				var addRole = new UserRolse()
				{
					UserId = users.Id,
					RoleId = roles.Id
				};


				await dbContext.AddAsync(addRole);
				await dbContext.SaveChangesAsync();

				return true;
			}
			return false;
		}

		public async Task<string> CreateRoleAsync( string roleName)
		{
			if (String.IsNullOrEmpty(roleName))
			{
				return "No Sucsses";
			}
			var result = new Role()
			{
				Name = roleName
			};
			if (result==null)
			{
				return "No Sucsses";
			}
			this.dbContext.Add(result);
			await this.dbContext.SaveChangesAsync();

			return "Sucsses";
		}

		public string FindRoleAsync(string rolName)
		{
			if (String.IsNullOrEmpty(rolName))
			{
				return "null";
			}

			var result =  this.dbContext.Roles.Where(r=>r.Name==rolName).FirstOrDefault();

			if (result==null)
			{
				return "null";
			}

			return result.Name;
		}
	}
}
