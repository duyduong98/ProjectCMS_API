﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectCMS.Data;
using ProjectCMS.Models;
using ProjectCMS.ViewModels;
using System.Text.Json;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace ProjectCMS.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        // Get all comments of an idea
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAllComments([FromRoute] int id)
        {
            var comments = await _dbContext._comments.Where(x=>x.IdeaId == id).ToListAsync();
            if (comments.Any())
                return Ok(comments);
            return NotFound();
        }

        // Add a comment
        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentViewModel comment)
        {
                if (ModelState.IsValid)
                {
                    Comment newComment = new()
                    {
                        Content = comment.Content,
                        IdeaId = comment.IdeaId,
                        UserId = comment.UserId,
                        AddedDate = DateTime.Now
                    };
                    await _dbContext._comments.AddAsync(newComment);
                    await _dbContext.SaveChangesAsync();
                    return Ok(await _dbContext._comments.Where(x => x.IdeaId == comment.IdeaId).ToListAsync());
                }
                return BadRequest(JsonDocument.Parse("{\"Message\":\"Some value is not valid. Please retype the value.\"}"));   
         }


        // Delete comment
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var comment = await _dbContext._comments.FindAsync(id);
            if (comment != null)
            {
                _dbContext._comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
                return Ok(await _dbContext._comments.ToListAsync());
            }
            return NotFound(JsonDocument.Parse("{\"Message\":\"Comment does not exist.\"}"));
        }


        // Edit comment
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> EditComment(CommentViewModel newComment, [FromRoute] int id)
        {
            var comment = await _dbContext._comments.FindAsync(id);
            if (comment != null)
            {
                if (ModelState.IsValid)
                {
                    comment.Content = newComment.Content;
                    await _dbContext.SaveChangesAsync();
                    return Ok(await _dbContext._comments.ToListAsync());
                }
                return BadRequest(JsonDocument.Parse("{\"Message\":\"Some value is not valid. Please retype the value.\"}"));
            }
            return NotFound(JsonDocument.Parse("{\"Message\":\"Comment does not exist.\"}"));
        }



        // Sort comment by added date 
        [HttpGet("{sort}/{id}")]
        public async Task<IActionResult> SortComments([FromRoute] int id)
        {
            try
            {
                var comments = _dbContext._comments.Where(x => x.IdeaId == id).OrderByDescending(x => x.AddedDate);
                if (comments.Any())
                    return Ok(await comments.ToListAsync());
                return NotFound(JsonDocument.Parse("{\"Message\":\"This idea has no comment yet.\"}"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        
              
 




    }
}

