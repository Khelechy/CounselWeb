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
