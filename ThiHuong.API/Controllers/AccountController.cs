using Microsoft.AspNetCore.Mvc;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiHuong.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace ThiHuong.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ThiHuongController
    {
        private IAccountService service;

        public AccountController(ExtensionSettings extensionSettings, IAccountService service) : base(extensionSettings)
        {
            this.service = service;
        }

        [HttpPost("login")]
        public dynamic Login([FromBody] UserAuthentication user)
        {
            return ExecuteInMonitoring(() =>
            {
                var result = service.Authen(user);
                return result;
            });
        }

        [HttpPost("register")]
        public async Task<dynamic> Register(UserRegisterdViewModel user)
        {
            return await ExecuteInMonitoring(async () =>
            {
                await this.service.Register(user);
                return new { success = "success" };
            });
        }

        [HttpGet]
        public string get()
        {
            return "account";
        }

    }
}
