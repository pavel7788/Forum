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
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        // GET: api/comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentModel>>> Index()
        {
            //return Ok(await _commentService.GetAllAsync());
            return Ok(await _commentService.GetAllWithDetailsAsync());
        }
        //GET: api/comments/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentModel>> Details(int id)
        {
            try
            {
                //await _commentService.GetByIdAsync(id);
                await _commentService.GetByIdWithDetailsAsync(id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            //return Ok(await _commentService.GetByIdAsync(id));
            return Ok(await _commentService.GetByIdWithDetailsAsync(id));
        }
        // POST: api/comments
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CommentModel value)
        {
            try
            {
                await _commentService.AddAsync(value);
            }
            catch (ForumException e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(Details), new { id = value.Id }, value);
        }
        // PUT: api/comments/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CommentModel value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }
            try
            {
                await _commentService.UpdateAsync(value);
            }
            catch (ForumException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }

        // DELETE: api/comments/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _commentService.DeleteAsync(id);
            }
            catch (ForumException e)
            {
                return BadRequest(e.Message);
            }
            return Ok(_commentService.DeleteAsync(id));
        }
    }
}
