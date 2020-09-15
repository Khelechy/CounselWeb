using CounselWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Services
{
	public interface IUserService
	{
		bool SaveChanges();
		Task<IEnumerable<User>> GetAllUsers();
		Task<User> GetUserById(int id);
		void CreateUser(User user);

		Task<User> Login(string email, string password);
	}
}
