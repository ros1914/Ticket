namespace RTSTicket.Service.Implementation
{
	using Microsoft.IdentityModel.Tokens;
	using RTSTicket.Data;
	using RTSTicket.Data.Models;
	using RTSTicket.Service.Models.Acount;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;
	using System.Security.Claims;
	using System.Security.Policy;
	using System.Text;
	using System.Text.Encodings.Web;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;

	public class AcountService : IAcountServices
	{

		private readonly RTSTicketDbContext db;

		public AcountService(RTSTicketDbContext db)
		{
			this.db = db;
		}

		public async Task<string> ConfirmEmail(string email, string token)
		{
			var user = this.db.Users.Where(p => p.Email == email).FirstOrDefault();
			if (user != null)
			{
				user.ConfirmEmail = true;
				await db.SaveChangesAsync();
				return "succses";

			}
			return "user is null";
		}

		public async Task<User> FindByEmailAsync(string email)
		{
			var user = this.db.Users.Where(u => u.Email == email).FirstOrDefault();

			if (user == null)
			{
				return null;
			}

			return user;

		}

		public async Task<User> Login(LoginModel loginModel)
		{
			var userAcount = db.Users.Where(b => b.UserName.Equals(loginModel.UserName)).FirstOrDefault();
			if (userAcount != null)
			{
				if ( BCrypt.Net.BCrypt.Verify(loginModel.Password, userAcount.Password))
				{
					return userAcount;
				}
				return null;
			}
			return null;
		}

		public async Task<bool> RegisterUserAsync(RegisterBindingModel model, string securityStamp)
		{
			if (model.Password != model.ConfirmPassword || String.IsNullOrEmpty(securityStamp))
			{
				return false;
			}

			var newUser = new User()
			{
				UserName = model.UserName,
				Email = model.Email,
				Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
				ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(model.Password),
				SecurityStamp = securityStamp

			};
			if (newUser == null)
			{
				return false;
			}
			this.db.Add(newUser);
			await db.SaveChangesAsync();
			return true;

		}

		public string TokenProvider()
		{
			//Authentication successful, Issue Token with user credentials
			//Provide the security key which was given in the JWToken configuration in Startup.cs
			var key = Encoding.ASCII.GetBytes
					  ("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
			//Generate Token for user 
			var JWToken = new JwtSecurityToken(
				issuer: "https://localhost:44340/",
				audience: "https://localhost:44340/",
				//claims: GetUserClaims(user),
				notBefore: new DateTimeOffset(DateTime.Now).DateTime,
				expires: new DateTimeOffset(DateTime.Now.AddMinutes(10)).DateTime,
				//Using HS256 Algorithm to encrypt Token
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
									SecurityAlgorithms.HmacSha256Signature)
			);
			var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
			return token;
		}


		public string CreateRandomPassword()
		{
			//string validChars = String.Join("", Enumerable.Range(33, (126 - 33)).Where(i => !(new int[] { 34, 38, 39, 44, 60, 62, 96 }).Contains(i)).Select(i => { return (char)i; }));
			string validChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
			return (string.Join("", Enumerable.Range(1, 8).Select(i => { return validChars[(new Random(Guid.NewGuid().GetHashCode())).Next(0, validChars.Length - 1)]; })));

		}

		public bool IsEmailConfirmedAsync(User user)
		{
			if (user == null)
			{
				return false;
			}

			var result = user.ConfirmEmail;

			if (result)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<string> SaveRandomPassword(string newRandomPasword, string userEmail)
		{
			if (String.IsNullOrEmpty(newRandomPasword) || String.IsNullOrEmpty(userEmail))
			{
				return "unsuccessful";
			}
			var user = FindUserByEmail(userEmail);
			if (user == null)
			{
				return "unsuccessful";
			}

			var hashNewRandomPassword = BCrypt.Net.BCrypt.HashPassword(newRandomPasword);

			user.Password = hashNewRandomPassword;
			user.ConfirmPassword = hashNewRandomPassword;
			await this.db.SaveChangesAsync();
			return "sucsses";
		}

		private User FindUserByEmail(string userEmail)
		{
			var user = this.db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
			return user;
		}

		public async Task<string> SaveForgotPassword(ResetPasswordModel model)
		{
			if (model.NewPassword != model.ConfirmNewPassword)
			{
				return "null";
			}

			var user = this.db.Users.Where(u => u.SecurityStamp == model.Code).FirstOrDefault();

			if (user != null)
			{
				if (BCrypt.Net.BCrypt.Verify(model.SendPasword, user.Password))
				{
					var hashNewPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
					user.Password = hashNewPassword;
					user.ConfirmPassword = hashNewPassword;
					user.SecurityStamp = SecurityStampTokenProvider();
					await this.db.SaveChangesAsync();
					return "sucsses";
				}
				user.SecurityStamp = SecurityStampTokenProvider();
			}

			return "null";
		}

		public string SecurityStampTokenProvider()
		{
			string validChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
			var userKey = (string.Join("", Enumerable.Range(1, 20).Select(i => { return validChars[(new Random(Guid.NewGuid().GetHashCode())).Next(0, validChars.Length - 1)]; })));
			//Authentication successful, Issue Token with user credentials
			//Provide the security key which was given in the JWToken configuration in Startup.cs
			var key = Encoding.ASCII.GetBytes
					  (userKey);
			//Generate Token for user 
			var JWToken = new JwtSecurityToken(
				issuer: "https://localhost:44340/",
				audience: "https://localhost:44340/",
				//claims: GetUserClaims(user),
				notBefore: new DateTimeOffset(DateTime.Now).DateTime,
				expires: new DateTimeOffset(DateTime.Now.AddMinutes(10)).DateTime,
				//Using HS256 Algorithm to encrypt Token
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
									SecurityAlgorithms.HmacSha256Signature)
			);
			var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
			return token;
		}


		//private IEnumerable<Claim> GetUserClaims(User user)
		//{
		//	IEnumerable claims = new Claim[]
		//	 {
		//			new Claim(ClaimTypes.Name, user.UserName),
		//			new Claim("EMAILID", user.Email),
		//			new Claim("ACCESS_LEVEL", user.Access_Level),
		//			new Claim("READ_ONLY", user.Read_Only)
		//	 };
		//	return claims;
		//}
	}
}
