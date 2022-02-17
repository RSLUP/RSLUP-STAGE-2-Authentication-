using AuthDemo.Interface;
using AuthDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace AuthDemo.Services
{
    public class AuthService : IAuthService
    {
        private readonly DbContext _dbContext;
        private readonly IConfiguration _config;
        public AuthService(DbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _config = configuration;
        }

        public LoginResponseModel Login(LoginRequestModel model)
        {
            model.Password = model.Password.ToMd5();

            var user = _dbContext.Set<User>().FirstOrDefault(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password));

            if (user == null)
            {
                throw new Exception("Username or password wrong");
            }

            return new LoginResponseModel
            {
                UserId = user.Id,
                Token = GenerateToken(user)
            };

        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Issuer"],
        claims,
        expires: DateTime.Now.AddMinutes(120),
        signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User Register(User model)
        {
            model.Password = model.Password.ToMd5();
            _dbContext.Set<User>().Add(model);
            _dbContext.SaveChanges();
            return model;
        }

        public User GetUser(int id)
        {
            return _dbContext.Set<User>().First(x => x.Id == id);
        }
    }
}
