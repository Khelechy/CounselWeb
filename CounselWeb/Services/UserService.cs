using CounselWeb.Data;
using CounselWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Services
{
	public class UserService : IUserService
	{
		private readonly CounselContext _context;

		public UserService(CounselContext context)
		{
			_context = context;
		}


		public async void ChangePassword(int id, string oldPassword, string newPassword)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if(user.Password == oldPassword)
			{
				user.Password = newPassword;
			}
		}

		public async void CreateUser(User user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			await _context.Users.AddAsync(user);
		}

		public async Task<IEnumerable<User>> GetAllUsers()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<IEnumerable<Notification>> GetNotifications(int userId)
		{
			return await _context.Notifications.Where(x => x.RId == userId).ToListAsync();
		}

		public async Task<int> GetNotificationsCount(int userId)
		{
			return await _context.Notifications.Where(x => x.RId == userId && x.IsRead == false).CountAsync();
		}

		public void SetAllRead(int userId)
		{
			var notify = _context.Notifications.Where(x => x.RId == userId && x.IsRead == false);
			foreach(var n in notify)
			{
				n.IsRead = true;
			}
			_context.SaveChanges();
		}

		public async Task<User> GetUserById(int id)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
		}

		public async Task<User> Login(string email, string password)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
				return null;
			var user =  await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
			if (user == null)
				return null;

			return user;
		}

		public bool SaveChanges()
		{
			return ( _context.SaveChanges() >= 0);
		}

		
	}
}
