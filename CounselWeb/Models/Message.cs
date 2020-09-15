using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Models
{
	public class Message
	{
		public int Id { get; set; }
		public int RequestId { set; get; }
		public string MessageBody { get; set; }
		public DateTime created_at { get; set; }
	}
}
