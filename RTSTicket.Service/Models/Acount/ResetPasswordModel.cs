using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RTSTicket.Service.Models.Acount
{
	public class ResetPasswordModel
	{
		public string Code { get; set; }

		[Required]
		[Display(Name = "Send Password")]
		[DataType(DataType.Password)]
		[MinLength(8, ErrorMessage = "Password length must be more !!!")]
		public string SendPasword { get; set; }

		[Required]
		[Display(Name = "New Password")]
		[MinLength(3, ErrorMessage = "Password length must be more 2 symbols !!!")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required]
		[Display(Name ="Confirm New Password")]
		[MinLength(3)]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "The new password and confirm new password password do not match.")]
		public string ConfirmNewPassword { get; set; }
	}
}
