using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Data;
using Data.Entities;
using Business.Interfaces;
using Business.Models;
using Business.Validation;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostModel>>> Index()
        {
            //return Ok(await _postService.GetAllAsync());
            return Ok(await _postService.GetAllWithDetailsAsync());
        }
        //GET: api/posts/1
        [HttpGet("{id}")]
        public async Task<ActionResult<PostModel>> Details(int id)
        {
            try
            {
                //await _postService.GetByIdAsync(id);
                return Ok(await _postService.GetByIdWithDetailsAsync(id));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }            
        }
        // POST: api/posts
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PostModel value)
        {
            try
            {
                await _postService.AddAsync(value);
            }
            catch (ForumException e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(Details), new { id = value.Id }, value);
        }
        // PUT: api/posts/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PostModel value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }
            try
            {
                await _postService.UpdateAsync(value);
            }
            catch (ForumException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }
        // DELETE: api/posts/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _postService.DeleteAsync(id);
            }
            catch (ForumException e)
            {
                return BadRequest(e.Message);
            }
            return Ok(_postService.DeleteAsync(id));
        }

        //GET: api/posts/1/comments
        [HttpGet("{id}/comments")]
        public async Task<ActionResult<PostModel>> GetCommentsByPostIdAsync (int id)
        {
            try
            {
                await _postService.GetCommentsByPostIdAsync(id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            return Ok(await _postService.GetCommentsByPostIdAsync(id));
        }
    }
}
