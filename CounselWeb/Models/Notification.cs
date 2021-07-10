using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Models
{
	public class Notification
	{
		public int Id { get; set; }
		public int? RId { get; set; }
		public int RequestId { get; set; }
		public string SenderName { get; set; }
		public bool IsRead { get; set; }
		public DateTime created_at { get; set; }
	}
}
