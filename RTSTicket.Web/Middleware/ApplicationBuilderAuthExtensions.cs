using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RTSTicket.Data.Models;
using RTSTicket.Service;
using RTSTicket.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTSTicket.Web.Middleware
{
	public static class ApplicationBuilderAuthExtensions
	{
		private const string DefaultAdminPassword = "admin123";

		private static readonly string[] roles ={"Administrator","Customer"};

		public static async void SeedDataBase( this IApplicationBuilder app)
		{
			var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
			var scoupe = serviceFactory.CreateScope();

			using (scoupe)
			{
				var acountService = scoupe.ServiceProvider.GetRequiredService<IAcountServices>();
				var adminService = scoupe.ServiceProvider.GetRequiredService<IAdminService>();

				foreach (var item in roles)
				{
					var findRole = adminService.FindRoleAsync(item);
					if (findRole != item)
					{
						await adminService.CreateRoleAsync(item);
					}
				}


				var user = acountService.FindByEmailAsync("admin@example.com");

				if (user == null)
				{
					var users = new User ()
					{
						UserName = "admin",
						Email= "admin@example.com",
						Password=DefaultAdminPassword,
						ConfirmPassword = DefaultAdminPassword,
						ConfirmEmail=true
					};

					await acountService.RegisterAdmin(users);
					await adminService.AddTorole(users,roles[0]);
				}
			}
		}

	}
}
