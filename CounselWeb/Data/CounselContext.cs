using CounselWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Data
{
	public class CounselContext : DbContext
	{
		public CounselContext(DbContextOptions<CounselContext> option) : base(option)
		{

		}

		public DbSet<User> Users { set; get; }
		public DbSet<Request> Requests { set; get; }

		public DbSet<Issue> Issues { set; get; }

		public DbSet<Message> Messages { set; get; }
	}
}
