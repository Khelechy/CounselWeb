using AutoMapper;
using CounselWeb.Dtos;
using CounselWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CounselWeb.Profiles
{
	public class UsersProfile : Profile
	{
		public UsersProfile()
		{
			CreateMap<RegisterDto, User>();
		}
	}
}
