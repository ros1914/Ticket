using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RTSTicket.Data.Models
{
	public class User
	{
		
		public long Id { get; set; }

		[Required]
		[MinLength(2)]
		[MaxLengthAttribute(20)]
		[Display(Name = "User Name")]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(3, ErrorMessage = "Password length must be more 2 symbols !!!")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[MinLength(3)]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public bool ConfirmEmail { get; set; }

		public string SecurityStamp { get; set; }

		public IEnumerable<UserRolse> Rolses { get; set; } = new List<UserRolse>();

	}
}
