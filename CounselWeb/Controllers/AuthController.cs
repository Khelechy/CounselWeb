using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CounselWeb.Dtos;
using CounselWeb.Models;
using CounselWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CounselWeb.Controllers
{
	public class AuthController : Controller
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public AuthController(IUserService userService, IMapper mapper)
		{
			_userService = userService;
			_mapper = mapper;
		}
		public IActionResult Index()
		{
			
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(LoginModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var user = _userService.Login(model.Email, model.Password);
					if (user == null)
					{
						return Redirect("/Login");
					}

					return Redirect("/User");
				}
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}


			return Redirect("/User");
		}

		public IActionResult Register(RegisterDto registerDto)
		{
			var registerModel = _mapper.Map<User>(registerDto);
			_userService.CreateUser(registerModel);
			var isRegistered = _userService.SaveChanges();
			if(isRegistered != true)
				return BadRequest(new { message = "could not register" });
			return RedirectToAction(nameof(Index));
		}
	}
}
