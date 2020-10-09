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
		public IActionResult Index()
		{
			var sessionUserId = this.HttpContext.Session.GetString("userid");
			ViewBag.name = this.HttpContext.Session.GetString("user_name");
			if (string.IsNullOrEmpty(sessionUserId))
			{
				return RedirectToAction(nameof(Login));
			}
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
			var requestItems = await _requestService.GetUserRequest(uid);
			return View(requestItems);
		}


		[HttpGet]
		public async Task<IActionResult> Messages(int Id)
		{
			ViewBag.name = this.HttpContext.Session.GetString("user_name");
			int uid = int.Parse(HttpContext.Session.GetString("userid"));
			int rId = Id;
			var messageItems = await _messageService.GetMessages(rId);
			ViewBag.reqId = rId;

			return View(messageItems);
		}


		[HttpPost]
		public IActionResult SendMessage(string messageBody, int requestId)
		{
			var name = this.HttpContext.Session.GetString("user_name");
			try
			{
				if (ModelState.IsValid)
				{
					var requestBody = new Message
					{
						RequestId = requestId,
						MessageBody = name + ": "  +messageBody,
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


		private void PopulateIssuesDropDownList(object selectedIssue = null)
		{
			var issuesQuery = from d in _context.Issues
								   orderby d.Name
								   select d;
			ViewBag.IssuesID = new SelectList(issuesQuery.AsNoTracking(), "Id", "Name", selectedIssue);
		}

	}
}
