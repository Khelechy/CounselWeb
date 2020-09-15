using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Models
{
	public class RegisterModel
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }
		public string MatricNo { get; set; }

		[Required]
		public string Password { get; set; }
		public string Department { get; set; }

		[Required]
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
	}
}
