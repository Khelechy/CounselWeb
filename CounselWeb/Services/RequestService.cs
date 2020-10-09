using CounselWeb.Data;
using CounselWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Services
{
	public class RequestService : IRequestService
	{
		private readonly CounselContext _context;

		public RequestService(CounselContext context)
		{
			_context = context;
		}

		public void AcceptRequest(int id, int adminId)
		{
			var request = _context.Requests.FirstOrDefault(r => r.Id == id);
			request.Status = Models.Enums.Status.Active;
			request.AdminId = adminId;

		}

		public async void CreateRequest(Request request)
		{

			if (request == null)
			{
				throw new ArgumentNullException(nameof(request));
			}
			await _context.Requests.AddAsync(request);
		}

		public async Task<IEnumerable<Request>> GetAdminRequests(int id)
		{
			var req = await _context.Requests.ToListAsync();
			return req.Where(x => x.AdminId == id);

		}

		public async Task<IEnumerable<Request>> GetAllRequest()
		{
			return await _context.Requests.ToListAsync();
		}

		public async Task<Request> GetRequestById(int id)
		{
			return await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task<IEnumerable<Request>> GetUserRequest(int id)
		{
			var req = await _context.Requests.ToListAsync();
			return req.Where(x => x.UserId == id);
		}

		public bool SaveChanges()
		{
			return (_context.SaveChanges() >= 0);
		}
	}
}
