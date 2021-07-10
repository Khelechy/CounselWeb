using CounselWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Services
{
	public interface IMessageService
	{
		bool SaveChanges();
		Task<IEnumerable<Message>> GetMessages(int requestId);

		void SendMessage(Message message);
		void AddNotification(Notification notification);
	}

}
