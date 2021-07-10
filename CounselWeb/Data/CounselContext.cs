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
		public DbSet<Notification> Notifications { set; get; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData(
				new User
				{
					Id = 1,
					FirstName = "Admin",
					LastName = "SuperAdmin",
					Email = "admin@counsel.com",
					IsAdmin = true,
					Password = "admin123"
				}
			);

			modelBuilder.Entity<Issue>().HasData(
				new Issue
				{
					Id = 1,
					Name = "Smoking"
				},
				new Issue
				{
					Id = 2,
					Name = "Career and course choices"
				},
				new Issue
				{
					Id = 3,
					Name = "Project choice"
				},
				new Issue
				{
					Id = 4,
					Name = "Depression and suicidal thoughts"
				},
				new Issue
				{
					Id = 5,
					Name = "Constant failing of courses"
				},
				new Issue
				{
					Id = 6,
					Name = "Prostitution"
				}
			);
		}
	}
}
