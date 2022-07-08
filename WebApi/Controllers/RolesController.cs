using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly string[] _baseRoles;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _baseRoles = new string[] { "Admin", "User", "AdvancedUser", "PowerUser", "Client", "SilverClient", "PlatinumClient" };
        }

        // GET: api/roles
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return Ok(await _roleManager.Roles.ToListAsync());
        }
        //GET: api/roles/1
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                await _roleManager.FindByIdAsync(id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            return Ok(await _roleManager.FindByIdAsync(id));
        }
        // POST: api/roles
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] IdentityRole value)
        {
            try
            {
                value.Id = Guid.NewGuid().ToString();
                await _roleManager.CreateAsync(value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(Details), new { id = value.Id }, value);
        }
        // PUT: api/roles/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] IdentityRole value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            string [] baseRolesIds = new string [] { };
            foreach (var name in _baseRoles)            
                baseRolesIds.Append((await _roleManager.FindByNameAsync(name)).Id);
            if (baseRolesIds.Contains(value.Id))
                return Forbid();

            try
            {
                await _roleManager.UpdateAsync(value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
        // DELETE: api/roles/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            IdentityRole role;
            try
            {
                role = await _roleManager.FindByIdAsync(id);
                if (_baseRoles.Contains(role.Name))
                    return Forbid();

                    await _roleManager.DeleteAsync(role);                    
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(await _roleManager.DeleteAsync(role));
        }

    }
}
