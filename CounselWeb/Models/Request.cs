using CounselWeb.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Models
{
	public class Request
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public string FullName { get; set; }

		[Required]
		public string Email { get; set; }
		[Required]
		public string MatricNo { get; set; }
		[Required]
		public string Problem { get; set; }
		[Required]
		public string Department { get; set; }

		[Required]
		public Status? Status { get; set; }

		public int? AdminId { set; get; }
	}
}
