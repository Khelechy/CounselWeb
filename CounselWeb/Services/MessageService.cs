using CounselWeb.Data;
using CounselWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Services
{
	public class MessageService : IMessageService
	{
		private readonly CounselContext _context;

		public MessageService(CounselContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Message>> GetMessages(int requestId)
		{
			var message = await _context.Messages.Where(m => m.RequestId == requestId).OrderBy(m => m.created_at).ToListAsync();
			return message;
		}

		public async void SendMessage(Message message)
		{
			if (message == null)
			{
				throw new ArgumentNullException(nameof(message));
			}
			await _context.Messages.AddAsync(message);
		}

		public bool SaveChanges()
		{
			return (_context.SaveChanges() >= 0);
		}

		public async void AddNotification(Notification notification)
		{
			if (notification == null)
			{
				throw new ArgumentNullException(nameof(notification));
			}

			await _context.Notifications.AddAsync(notification);
		}
	}
}
