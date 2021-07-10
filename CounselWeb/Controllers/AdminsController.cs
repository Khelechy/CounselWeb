using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CounselWeb.Models;
using CounselWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CounselWeb.Controllers
{
	public class AdminsController : Controller
	{
		private readonly IUserService _userService;
		private readonly IRequestService _requestService;
		private readonly IMessageService _messageService;

		public AdminsController(IUserService userService, IRequestService requestService, IMessageService messageService)
		{
			_userService = userService;
			_requestService = requestService;
			_messageService = messageService;
		}
		public async Task<IActionResult> Index()
		{
			int adminId = int.Parse(HttpContext.Session.GetString("userid"));
			var userItems = await _userService.GetAllUsers();
			var requestItems = await _requestService.GetAllRequest();
			var totalUsers = userItems.Count();
			var totalRequest = requestItems.Count();
			var pendingRequest = requestItems.Where(x => x.Status == Models.Enums.Status.Pending).Count();
			var completedRequest = requestItems.Where(x => x.Status == Models.Enums.Status.Completed).Count();

			var statisticModel = new Statistic
			{
				TotalRequest = totalRequest,
				TotalUsers = totalUsers,
				TotalPending = pendingRequest,
				TotalCompleted = completedRequest
			};
			var notificationsCount = await _userService.GetNotificationsCount(adminId);
			ViewBag.ncount = notificationsCount;
			return View(statisticModel);
		}

		public async Task<ActionResult> Users()
		{
			int adminId = int.Parse(HttpContext.Session.GetString("userid"));
			var notificationsCount = await _userService.GetNotificationsCount(adminId);
			ViewBag.ncount = notificationsCount;
			var userItems = await _userService.GetAllUsers();
			return View(userItems);
		}

		public async Task<ActionResult> Requests()
		{
			int adminId = int.Parse(HttpContext.Session.GetString("userid"));
			var notificationsCount = await _userService.GetNotificationsCount(adminId);
			ViewBag.ncount = notificationsCount;
			var requestItems = await _requestService.GetAllRequest();
			return View(requestItems);
		}

		public async Task<ActionResult> Active()
		{
			int adminId = int.Parse(HttpContext.Session.GetString("userid"));
			var notificationsCount = await _userService.GetNotificationsCount(adminId);
			ViewBag.ncount = notificationsCount;
			var activeItems = await _requestService.GetAdminRequests(adminId);
			return View(activeItems);

		}

		[HttpGet]
		public async Task<ActionResult> Notifications()
		{
			int uid = int.Parse(HttpContext.Session.GetString("userid"));
			ViewBag.name = this.HttpContext.Session.GetString("user_name");
			_userService.SetAllRead(uid);
			var notifications = await _userService.GetNotifications(uid);
			notifications.OrderBy(n => n.created_at);
			return View(notifications);
		}

		[HttpGet]
		public async Task<IActionResult> Messages(int Id)
		{
			int rId = Id;
			int adminId = int.Parse(HttpContext.Session.GetString("userid"));
			var notificationsCount = await _userService.GetNotificationsCount(adminId);
			ViewBag.ncount = notificationsCount;
			var messageItems = await _messageService.GetMessages(rId);
			ViewBag.reqId = rId;

			return View(messageItems);
		}

		[HttpPost]
		public async Task<IActionResult> SendMessage(string messageBody, int requestId)
		{
			var name = this.HttpContext.Session.GetString("user_name");
			var request = await _requestService.GetRequestById(requestId);
			var rid = request.UserId;
			try
			{
				if (ModelState.IsValid)
				{
					var requestBody = new Message
					{
						RequestId = requestId,
						SenderName = name,
						MessageBody = messageBody,
						created_at = DateTime.Now

					};

					_messageService.SendMessage(requestBody);
					var notify = new Notification
					{
						RequestId = requestId,
						SenderName = name,
						created_at = DateTime.Now,
						RId = rid
					};
					_messageService.AddNotification(notify);
					var isRequested = _messageService.SaveChanges();
					if (isRequested != true)
						Console.WriteLine("Not sent");
					//	return BadRequest(new { message = "could not make request" });

					//return RedirectToAction("Messages", "Users", requestId);
				}
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}

			return RedirectToAction("Messages", new { Id = requestId });
			//return RedirectToAction("Messages");
		}


		public IActionResult Accept(int id)
		{
			int adminId = int.Parse(HttpContext.Session.GetString("userid"));
			_requestService.AcceptRequest(id, adminId);
			var isRequested = _requestService.SaveChanges();
			if (isRequested != true)
				return RedirectToAction(nameof(Index));
			return RedirectToAction(nameof(Requests));

		}

		public IActionResult Complete(int id)
		{
			_requestService.CompleteRequest(id);
			var isRequested = _requestService.SaveChanges();
			if (isRequested != true)
				return RedirectToAction(nameof(Index));
			return RedirectToAction(nameof(Requests));

		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Users");
		}


	}
}
