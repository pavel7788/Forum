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
                await _userService.GetByIdWithDetailsAsync(id);
                //await _userManager.FindByIdAsync(id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            return Ok(_userService.GetByIdWithDetailsAsync(id));
            //return Ok(await _userManager.FindByIdAsync(id));
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
                await _userService.DeleteCommentsByUserIdAsync(id);
                await _userManager.DeleteAsync(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(await _userManager.DeleteAsync(user));
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
    }
}
