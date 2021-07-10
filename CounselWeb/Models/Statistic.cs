using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Models
{
	public class Statistic
	{
		public int TotalUsers { get; set; }
		public int TotalRequest { get; set; }
		public int TotalPending { get; set; }
		public int TotalCompleted{ get; set; }
	}
}
