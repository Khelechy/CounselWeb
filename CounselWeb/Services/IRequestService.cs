using CounselWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Services
{
	public interface IRequestService
	{
		bool SaveChanges();
		Task<IEnumerable<Request>> GetAllRequest();

		Task<IEnumerable<Request>> GetUserRequest(int id);
		Task<Request> GetRequestById(int id);

		Task<IEnumerable<Request>> GetAdminRequests(int id);

		void AcceptRequest(int id, int adminId);
		void CompleteRequest(int id);
		void CreateRequest(Request request);
	}
}
