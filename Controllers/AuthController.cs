using AuthDemo.Interface;
using AuthDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _autService;
        public AuthController(IAuthService autService)
        {
            _autService = autService;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register(User model)
        {

            return Ok(_autService.Register(model));
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequestModel model)
        {

            return Ok(_autService.Login(model));
        }

        [HttpGet]
        [Route("GetUser")]
        [Authorize(Roles = "Admin,Studet")]
        public IActionResult GetUser(int id)
        {
            return Ok(_autService.GetUser(id));
        }

    }
}
