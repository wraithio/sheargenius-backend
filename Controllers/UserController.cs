using sheargenius_backend.Models;
using sheargenius_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sheargenius_backend.Models.DTOs;

namespace sheargenius_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // Remove this to host on swagger
    // [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("SeeAllUsers")]
        public List<UserModel> SeeAllUsers()
        {
            return _userServices.SeeAllUsers();
        }

        [HttpPost("CreateUser")]
        //[FromBody] attribute better directs to where data will be passed from 
        public async Task<IActionResult> CreateUser([FromBody] UserInfoDTO newUser)
        {
            var success = await _userServices.CreateUser(newUser);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Success = false, Message = "Username already exists..." });
            //above statement is being EVALUATED as a boolean, returns true or false
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDTO user)
        {
            string stringToken = await _userServices.Login(user);
            if (stringToken != null) return Ok(new { Token = stringToken });
            return Unauthorized(new { Message = "Login was unsuccessful. Invalid username or password." });

        }

        [HttpPut("EditAccount")]
        public async Task<IActionResult> EditAccount([FromBody] UserModel updatedUser)
        {
            var success = await _userServices.EditAccount(updatedUser);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "Changes have not been saved..." });
        }
        
        [HttpPut("ToggleFollowers")]
        public async Task<IActionResult> ToggleFollowers(string followingUser, string followedUser)
        {
            var success = await _userServices.ToggleFollowersAsync(followingUser, followedUser);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "Changes have not been saved..." });
        }

        // [HttpPut("UpdatePassword")]
        // public async Task<IActionResult> UpdatePassword([FromBody] UserDTO user)
        // {
        //     var success = await _userServices.UpdatePassword(user.)
        //     return _userServices.UpdatePassword(user);
        // }

        [HttpDelete]
        [Route("DeleteAccount")]

        public async Task<IActionResult> DeleteAccount([FromBody] UserDTO user)
        {
            var success = await _userServices.DeleteAccount(user);
            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "Changes have not been saved..." });
        }

        // [HttpGet("GetUserByUsername/{username}")]
        // public async Task<IActionResult> GetUserByUsername(string username)
        // {
        //     var user = await _userServices.GetUserByUsername(username);

        //     if(user != null) return Ok(user);
        //     return BadRequest(new {Message = "No user found..."});
        // }

        [HttpGet("GetUserInfoByUsername/{username}")]
        public async Task<IActionResult> GetUserInfoByUsername(string username)
        {
            // var user = await _userServices.GetUserInfoByUsername(username);
            var user = await _userServices.GetUserByUsername(username);

            if (user != null) return Ok(user);
            return BadRequest(new { Message = "No user found..." });
        }

         [HttpGet("GetProfileInfoByUsername/{username}")]
        public async Task<IActionResult> GetProfileInfoByUsername(string username)
        {
            // var user = await _userServices.GetUserInfoByUsername(username);
            var user = await _userServices.GetProfileInfoByUsername(username);
            
            if (user != null) return Ok(user);
            return BadRequest(new { Message = "No user found..." });
        }
    }
}