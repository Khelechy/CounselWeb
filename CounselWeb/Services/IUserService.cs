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
		void ChangePassword(int id, string oldPassword, string newPassword);
		void CreateUser(User user);
		void SetAllRead(int userId);
		Task<IEnumerable<Notification>> GetNotifications(int userId);
		Task<int> GetNotificationsCount(int userId);

		Task<User> Login(string email, string password);
	}
}
