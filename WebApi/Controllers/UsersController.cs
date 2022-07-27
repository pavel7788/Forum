using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private int AdminCount {get=>GetAdminCount().Result;}
        public UsersController(IUserService userService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> Index()
        {
            //return Ok(await _userManager.Users.ToListAsync());
            return Ok(await _userService.GetAllWithDetailsAsync());
        }        
        //GET: api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> Details(string id)
        {
            try
            {
                //выдает ошибку циклической ссылки!!!
                //await _userService.GetByIdWithDetailsAsync(id);
                await _userManager.FindByIdAsync(id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            //return Ok(_userService.GetByIdWithDetailsAsync(id));
            return Ok(await _userManager.FindByIdAsync(id));
        }
        // POST: api/users
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] User value)
        {
            try
            {
                await _userManager.CreateAsync(value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(Details), new { id = value.Id }, value);
        }
        // PUT: api/users/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] User value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }
            try
            {
                await _userManager.UpdateAsync(value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
        // DELETE: api/users/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {        
            User user;
            try
            {
                user = await _userManager.FindByIdAsync(id);

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Contains("Admin") && AdminCount == 1)
                    return Forbid();

                await _userService.DeleteCommentsByUserIdAsync(id);
                await _userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(await _userManager.DeleteAsync(user));
        }
        // GET: api/users/1/role
        [HttpGet("{id}/role")]
        public async Task<ActionResult> GetRoleByUserIdAsync(string id)
        {
            User user;
            List<string> roleNames = new List<string>();
            try
            {
                user = await _userManager.FindByIdAsync(id);
                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in userRoles)
                    roleNames.Add(role.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            //return Ok(await _userManager.GetRolesAsync(user));
            return Ok(roleNames[0]);
        }

        // GET: api/users/1/posts
        [HttpGet("{id}/posts")]
        public async Task<ActionResult<IEnumerable<PostModel>>> GetPostsWithDetailsByUserIdAsync(string id)
        {
            return Ok(await _userService.GetPostsWithDetailsByUserIdAsync(id));
        }

        // GET: api/users/1/posts/1/comments/1
        [HttpGet("{userId}/posts/{postId}/comments")]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetCommentsWithDetailsInUserPostAsync(string userId, int postId)
        {
            return Ok(await _userService.GetCommentsWithDetailsInUserPostAsync(userId, postId));
        }

        // PUT: api/users/1/ban
        [HttpPut("{id}/ban")]
        public async Task<ActionResult> Ban(string id)
        {
            User user;
            try
            {
                user = await _userManager.FindByIdAsync(id);

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Contains("Admin") && AdminCount == 1)
                    return Forbid();

                if (user.IsBanned == true)
                    user.IsBanned = false;
                else if (user.IsBanned == false)
                    user.IsBanned = true;
                await _userManager.UpdateAsync(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();         
        }


        private async Task<int> GetAdminCount ()
        {
            int c = 0;
            foreach (var user in _userManager.Users)
                if ((await _userManager.GetRolesAsync(user)).Contains("Admin"))
                    c++;
            return c;
        }
    }
}
