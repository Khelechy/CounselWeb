using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CounselWeb.Data;
using CounselWeb.Dtos;
using CounselWeb.Models;
using CounselWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CounselWeb.Controllers
{
	public class UsersController : Controller
	{
		private readonly IRequestService _requestService;
		private readonly IUserService _userService;
		private readonly IMessageService _messageService;
		private readonly IMapper _mapper;
		private readonly CounselContext _context;

		public UsersController(IRequestService requestService, IUserService userService, IMapper mapper, CounselContext context, IMessageService messageService)
		{
			_requestService = requestService;
			_userService = userService;
			_messageService = messageService;
			_mapper = mapper;
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			var sessionUserId = this.HttpContext.Session.GetString("userid");
			if (string.IsNullOrEmpty(sessionUserId))
			{
				return RedirectToAction(nameof(Login));
			}
			var uid = int.Parse(sessionUserId);
			ViewBag.name = this.HttpContext.Session.GetString("user_name");
			var notificationsCount = await _userService.GetNotificationsCount(uid);
			ViewBag.ncount = notificationsCount;
			
			PopulateIssuesDropDownList();
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> MakeRequest(RequestModel request)
		{
			try
			{
				if (ModelState.IsValid)
				{
					int id = int.Parse(HttpContext.Session.GetString("userid"));
					var issuesName = await _context.Issues.SingleOrDefaultAsync(i => i.Id == request.Issues);
					var user = await _userService.GetUserById(id);
					var requestBody = new Request
					{
						UserId = user.Id,
						FullName = user.FirstName + " " + user.LastName,
						Department = user.Department,
						Email = user.Email,
						MatricNo = user.MatricNo,
						Problem = request.Problem + ": " + issuesName.Name,
						Status = Models.Enums.Status.Pending
					};

					_requestService.CreateRequest(requestBody);
					var isRequested = _requestService.SaveChanges();
					if (isRequested != true)
						return BadRequest(new { message = "could not make request" });

					return RedirectToAction(nameof(Index));
				}
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}

			return RedirectToAction(nameof(Index));

		}

		public async Task<ActionResult> Requests()
		{
			int uid = int.Parse(HttpContext.Session.GetString("userid"));
			ViewBag.name = this.HttpContext.Session.GetString("user_name");
			var notificationsCount = await _userService.GetNotificationsCount(uid);
			ViewBag.ncount = notificationsCount;
			var requestItems = await _requestService.GetUserRequest(uid);
			return View(requestItems);
		}

		[HttpGet]
		public async Task<ActionResult> Profile()
		{
			int uid = int.Parse(HttpContext.Session.GetString("userid"));
			ViewBag.name = this.HttpContext.Session.GetString("user_name");
			var notificationsCount = await _userService.GetNotificationsCount(uid);
			ViewBag.ncount = notificationsCount;
			var user = await _userService.GetUserById(uid);
			var userModel = new UserViewModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Department = user.Department,
				MatricNo = user.MatricNo
			};
			return View(userModel);
		}

		[HttpGet]
		public async Task<ActionResult> Notifications()
		{
			int uid = int.Parse(HttpContext.Session.GetString("userid"));
			ViewBag.name = this.HttpContext.Session.GetString("user_name");
			_userService.SetAllRead(uid);
			var notifications = await _userService.GetNotifications(uid);
			return View(notifications);
		}

		[HttpPost]
		public ActionResult ChangePassword(UserViewModel model)
		{
			int uid = int.Parse(HttpContext.Session.GetString("userid"));
			_userService.ChangePassword(uid, model.OldPassword, model.NewPassword);
			var isChanged = _userService.SaveChanges();
			if (isChanged != true)
				return BadRequest(new { message = "could not change password" });
			return RedirectToAction(nameof(Profile));
		}


		[HttpGet]
		public async Task<IActionResult> Messages(int Id)
		{
			ViewBag.name = this.HttpContext.Session.GetString("user_name");
			int uid = int.Parse(HttpContext.Session.GetString("userid"));
			int rId = Id;
			var notificationsCount = await _userService.GetNotificationsCount(uid);
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
			var rid = request.AdminId;

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

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LoginPost(LoginModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var user = await _userService.Login(model.Email, model.Password);
					if (user != null)
					{
						HttpContext.Session.SetString("userid", user.Id.ToString());
						HttpContext.Session.SetString("user_name", user.FirstName + " " + user.LastName);
						if (user.IsAdmin == true)
						{
							return RedirectToAction("Index", "Admins");
						}
						else
						{
							return RedirectToAction(nameof(Index));
						}
						
					}
					else
					{
						TempData["Error"] = "Incorrect Email/Password";
						return RedirectToAction(nameof(Login));
					}
				}
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}


			return View();
		}


		public IActionResult Register()
		{
			return View();
		}

		public IActionResult RegisterPost(RegisterModel model)
		{

			try
			{
				if (ModelState.IsValid)
				{
					var usermodel = new User
					{
						FirstName = model.FirstName,
						LastName = model.LastName,
						MatricNo = model.MatricNo,
						Email = model.Email,
						IsAdmin = false,
						Department = model.Department,
						Password = model.Password
					};
					_userService.CreateUser(usermodel);
					var isRegistered = _userService.SaveChanges();
					if (isRegistered != true)
						return BadRequest(new { message = "could not register" });
					return RedirectToAction(nameof(Login));
				}
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}
			//var registerModel = _mapper.Map<User>(registerDto);
			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction(nameof(Login));
		}


		private void PopulateIssuesDropDownList(object selectedIssue = null)
		{
			var issuesQuery = from d in _context.Issues
								   orderby d.Name
								   select d;
			ViewBag.IssuesID = new SelectList(issuesQuery.AsNoTracking(), "Id", "Name", selectedIssue);
		}

	}
}
