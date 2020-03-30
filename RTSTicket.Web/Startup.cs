using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RTSTicket.Data;
using RTSTicket.Service;
using RTSTicket.Service.Implementation;
using RTSTicket.Web.Data;
using RTSTicket.Web.Filters;
using RTSTicket.Web.Middleware;
using RTSTicket.Web.Models;
using RTSTicket.Web.Services;

namespace RTSTicket.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.Lax;
			});

			services.AddDistributedMemoryCache();

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(10);
				options.Cookie.HttpOnly = true;
			});

			services.AddDbContext<RTSTicketDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});


			services.AddControllersWithViews();
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie();
			

			

			services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
			services.AddMvc(options => options.EnableEndpointRouting = false);

			services.AddAuthorization(options=>
			{
				//options.AddPolicy("Administrator", policy => policy.AddRequirements(new AddHedarAuthorizationFilter()));
			});
			//Provide a secret key to Encrypt and Decrypt the Token
			var SecretKey = Encoding.ASCII.GetBytes
				 ("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
			//Configure JWT Token Authentication
			services.AddAuthentication(auth =>
			{
				auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(token =>
			{
				token.RequireHttpsMetadata = false;
				token.SaveToken = true;
				token.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
			//Same Secret key will be used while creating the token
			IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
					ValidateIssuer = true,
			//Usually, this is your application base URL
			ValidIssuer = "https://localhost:44340/",
					ValidateAudience = true,
			//Here, we are creating and using JWT within the same application.
			//In this case, base URL is fine.
			//If the JWT is created using a web service, then this would be the consumer URL.
			ValidAudience = "https://localhost:44340/",
					RequireExpirationTime = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});

			services.AddScoped<IAcountServices, AcountService>();
			services.AddScoped<IAdminService, AdminService>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<MyAuthorizationFilter>();
			services.AddSingleton<IEmailSender, SendGridEmailSender>();
			services.Configure<SendGridOptions>(this.Configuration.GetSection("EmailSettings"));
			services.Configure<StripeSettings>(this.Configuration.GetSection("Stripe"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.UseRouting();
			app.UseSession();
			//Add JWToken to all incoming HTTP Request Header
			app.Use(async (context, next) =>
			{
				var JWToken = context.Session.GetString("JWToken");
				if (!string.IsNullOrEmpty(JWToken))
				{
					context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
				}
				await next();
			});
			app.SeedDataBase();
			app.UseAuthorization();
			app.UseAuthentication();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapControllerRoute(
			//		name: "default",
			//		pattern: "{controller=Home}/{action=Index}/{id?}");
			//});
		}
	}
}
