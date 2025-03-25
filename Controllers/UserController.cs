using sheargenius_backend.Models;
using sheargenius_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sheargenius_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> CreateUser([FromBody] UserDTO newUser)
        {
            if(await _userServices.CreateUser(newUser)) return Ok(new {Success = true});
            return BadRequest(new {Success = false, Message = "Username already exists..."});
            //above statement is being EVALUATED as a boolean, returns true or false
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDTO user)
        {
            string stringToken = await _userServices.Login(user);
            if (stringToken != null) return Ok(new { Token = stringToken });
            return Unauthorized(new { Message = "Login was unsuccessful. Invalid email or password." });

        }

        [HttpPut("EditAccount")]
        public async Task<IActionResult> EditAccount([FromBody] UserModel updatedUser)
        {
            if(_userServices.EditAccount(updatedUser) != null) return Ok(new {Success = true});
            return BadRequest(new {Message = "Changes have not been saved..."});
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
            if(await _userServices.DeleteAccount(user) != null) return Ok(new {Success = true});
            return BadRequest(new {Message = "Changes have not been saved..."});
        }

        [Authorize]
        [HttpGet]
        [Route("AuthenticUser")]
        public string AuthenticUserCheck()
        {
            return "You are logged in and allowed to be here.";
        }
    }
}