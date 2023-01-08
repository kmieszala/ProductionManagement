using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Common.Consts;
using ProductionManagement.Common.Extensions;
using ProductionManagement.Services.Services.Users;
using ProductionManagement.Services.Services.Users.Models;
using ProductionManagement.WebUI.Areas.Authorization.ViewModels;
using Serilog;

namespace ProductionManagement.WebUI.Areas.Authorization
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public AuthorizationController(
            IUsersService usersService,
            IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("IsAuthenticated")]
        public IActionResult IsAuthenticated()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return Ok(User.Identity.IsAuthenticated);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Log.Information($"Logowanie {model.UserName}");
            try
            {
                LoginUserModel? user;
                if (!string.IsNullOrWhiteSpace(model.RepeatPassword))
                {
                    user = await _usersService.ChangePasswordAsync(model.UserName, model.Password, model.RepeatPassword);
                }
                else
                {
                    user = await _usersService.LoginAsync(model.UserName, model.Password);
                }

                if (user == null)
                {
                    return Ok(null);
                }

                #region Production profile

                var roles = user.UserRoles.Select(x => x.GetDisplayName()).ToList();

                Log.Information("OK 1");
                var claims = new List<Claim>
                    {
                        new Claim(CustomClaimTypesConsts.Login, model.UserName),
                        new Claim(CustomClaimTypesConsts.FirstName, user.FirstName),
                        new Claim(CustomClaimTypesConsts.LastName, user.LastName),
                        new Claim(CustomClaimTypesConsts.Roles, string.Join(";", roles)),
                    };

                Log.Information("OK 2");
                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                #endregion Production profile

                Log.Information("OK 3");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                Log.Information("OK 4");
                if (user.Status == Common.Enums.UserStatusEnum.Active)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                }
                return Ok(new { FirstName = user.FirstName, LastName = user.LastName, Roles = roles.Select(x => (string)x), Status = user.Status });
            }
            catch (Exception)
            {
                Log.Error("Błąd!!!");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(true);
        }

        [HttpGet]
        [Route("getusername/{id:int}")]
        public IActionResult GetUserName(int id)
        {
            return Ok(new { FirstName = "Tomek", LastName = "Nowak" });
        }
    }
}