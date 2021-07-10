using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Models
{
	public class UserViewModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MatricNo { get; set; }
		public string Department { get; set; }
		public string Email { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
	}
}
