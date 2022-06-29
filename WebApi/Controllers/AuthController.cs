using AuthCommon;
using AuthCommon.Models;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<AuthOptions> _authOptions;
   
        public AuthController(IUserService userService, IOptions<AuthOptions> authOptions,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _userService = userService;
            _authOptions = authOptions;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // POST: api/auth/login
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody]Login request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user !=null)
            {
                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                    return Unauthorized();

                var token = await GenerateJWT(user);

                return Ok(new
                {
                    access_token = token,
                    //id = user.Id,
                    //email = user.Email
                });
            }
            return Unauthorized();
        }

        // POST: api/auth/login
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> Register ([FromBody] Login request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                return BadRequest();
            }
            try
            {
                var addUser = new User()
                {
                    UserName = request.Email,
                    Email = request.Email
                };
                if ((await _userManager.CreateAsync(addUser, request.Password)).Succeeded)
                    await _userManager.AddToRoleAsync(addUser, "User");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }







        private async Task <string> GenerateJWT(User user)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role.ToString()));
            }

            var token = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
