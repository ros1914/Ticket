using RTSTicket.Data.Models;
using System.Threading.Tasks;

namespace RTSTicket.Service
{
	public interface IAdminService
	{
		Task<string> CreateRoleAsync(string roleName);

		string FindRoleAsync(string rolName);

		Task<bool> AddTorole(User user, string roleName);
	}
}
