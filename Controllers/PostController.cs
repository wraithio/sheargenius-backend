using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sheargenius_backend.Models;
using sheargenius_backend.Services;

namespace sheargenius_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostServices _postServices;

        public PostController(PostServices postServices)
        {
            _postServices = postServices;
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postServices.GetPostsAsync();
            if (posts != null) return Ok(posts);
            return BadRequest(new { Message = "No Posts" });
        }

        [HttpGet("GetAllComments")]
        public async Task<IActionResult> GetAllComments()
        {
            var posts = await _postServices.GetCommentsAsync();
            if (posts != null) return Ok(posts);
            return BadRequest(new { Message = "No Comments" });
        }

        [HttpGet("GetCommentsByPostId")]
        public async Task<IActionResult> GetCommentsByPostId(int id)
        {
            var posts = await _postServices.GetCommentsByPostIdAsync(id);
            if (posts != null) return Ok(posts);
            return BadRequest(new { Message = "No comments..." });
        }

        [HttpDelete("DeleteComment")]
        public async Task<IActionResult> DeleteCommentAsync(int id)
        {
            var success = await _postServices.DeleteCommentAsync(id);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "No comment was found..." });
        }
        

        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost([FromBody] PostModel post)
        {
            var success = await _postServices.AddPostAsync(post);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "Post was not added..." });
        }

        [HttpPut("EditPost")]
        public async Task<IActionResult> EditPost([FromBody] PostModel post)
        {
            var success = await _postServices.EditPostAsync(post);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "No post was found..." });
        }

        [HttpPut("ToggleLikes")]
        public async Task<IActionResult> ToggleLikes(int postId,string username)
        {
            var success = await _postServices.ToggleLikesAsync(postId,username);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "Like failed to add/remove..." });
        }
        
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CommentModel comment)
        {
            var success = await _postServices.AddCommentAsync(comment);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "No post was found..." });
        }

        [HttpPut("DeletePost")]
        public async Task<IActionResult> DeletePost([FromBody] PostModel post)
        {
            var success = await _postServices.EditPostAsync(post);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "No post was found..." });
        }

        [HttpDelete("AdminDeletePost")]
        public async Task<IActionResult> AdminDeletePost(int id)
        {
            var success = await _postServices.AdminDeletePostAsync(id);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "No post was found..." });
        }

        [HttpGet("GetPostsByUserId/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var posts = await _postServices.GetPostsByUserIdAsync(userId);
            if(posts != null) return Ok(posts);
            return BadRequest(new {Message = "No Posts"});
        }

        [HttpGet("GetPostsByCategory/{category}")]
        public async Task<IActionResult> GetPostsByCategory(string category)
        {
            var posts = await _postServices.GetPostsbyCategory(category);
            if(posts != null) return Ok(posts);
            return BadRequest(new {Message = "No posts have that category"});
        }
    }
}