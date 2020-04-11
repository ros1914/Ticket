namespace RTSTicket.Service
{
	using RTSTicket.Service.Models.Acount;
	using System.Threading.Tasks;
	using RTSTicket.Data.Models;
    using System.Collections.Generic;

    public interface IAcountServices
	{
		Task<bool> RegisterUserAsync(RegisterBindingModel model, string securityStamp);
		User Login(LoginModel loginModel);
		string TokenProvider();
		Task<string> ConfirmEmail(string email, string token);
		User FindByEmailAsync(string email);
		string CreateRandomPassword();
		bool IsEmailConfirmedAsync(User user);
		Task<string> SaveRandomPassword(string newRandomPasword, string userEmail);
		Task<string> SaveForgotPassword(ResetPasswordModel model);
		string SecurityStampTokenProvider();
		IEnumerable<string> GetAllRolesOnUser(long userId);
		Task<bool> RegisterAdmin(User user);
	}
}
