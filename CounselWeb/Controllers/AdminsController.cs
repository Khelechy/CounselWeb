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
			var userItems = await _userService.GetAllUsers();
			var requestItems = await _requestService.GetAllRequest();
			var totalUsers = userItems.Count();
			var totalRequest = requestItems.Count();

			var statisticModel = new Statistic
			{
				TotalRequest = totalRequest,
				TotalUsers = totalUsers
			};

			return View(statisticModel);
		}

		public async Task<ActionResult> Users()
		{
			var userItems = await _userService.GetAllUsers();
			return View(userItems);
		}

		public async Task<ActionResult> Requests()
		{
			var requestItems = await _requestService.GetAllRequest();
			return View(requestItems);
		}

		public async Task<ActionResult> Active()
		{
			int adminId = int.Parse(HttpContext.Session.GetString("userid"));
			var activeItems = await _requestService.GetAdminRequests(adminId);
			return View(activeItems);

		}

		[HttpGet]
		public async Task<IActionResult> Messages(int Id)
		{
			int rId = Id;
			var messageItems = await _messageService.GetMessages(rId);
			ViewBag.reqId = rId;

			return View(messageItems);
		}

		[HttpPost]
		public IActionResult SendMessage(string messageBody, int requestId)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var requestBody = new Message
					{
						RequestId = requestId,
						MessageBody = "Admin: " + messageBody,
						created_at = DateTime.Now

					};

					_messageService.SendMessage(requestBody);
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


	}
}
