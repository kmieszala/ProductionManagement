using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Common.Enums;
using ProductionManagement.Services.Services.Users;
using ProductionManagement.Services.Services.Users.Models;
using ProductionManagement.WebUI.Areas.Shared.Models;
using ProductionManagement.WebUI.Areas.Users.ViewModels;

namespace ProductionManagement.WebUI.Areas.Users
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(
            IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddUser(UsersVM model)
        {
            var result = await _usersService.AddUserAsync(_mapper.Map<UsersModel>(model));
            return Ok(_mapper.Map<UsersVM>(result));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> EditUser(UsersVM model)
        {
            var result = await _usersService.EditUserAsync(_mapper.Map<UsersModel>(model));
            return Ok(_mapper.Map<UsersVM>(result));
        }

        [HttpGet("[action]/{login}")]
        public async Task<IActionResult> CheckUniqueLogin(string login)
        {
            var result = await _usersService.CheckUniqueLoginAsync(login);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _usersService.GetUsersAsync();
            return Ok(_mapper.Map<IEnumerable<UsersVM>>(result));
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetRoles()
        {
            var result = Enum.GetValues(typeof(RolesEnum)).Cast<RolesEnum>().Select(x => new DictVM
            {
                Id = (int)x,
                Value = x.ToString()
            }).ToList();

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeactiveUser(DictVM model)
        {
            var result = await _usersService.DeactiveUserAsync(model.Id);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UnlockUser(DictVM model)
        {
            var result = await _usersService.UnlockUserAsync(model.Id, model.Value);
            return Ok(result);
        }
    }
}