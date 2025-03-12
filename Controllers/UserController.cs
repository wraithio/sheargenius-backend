using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        //dependency injection
        private readonly UserServices _userServices;

        //constructor
        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        [Route("SeeAllUsers")]
        public List<UserModel> SeeAllUsers(){
            return _userServices.SeeAllUsers();
        }

        [HttpPost]
        [Route("CreateUser")]

        //[FromBody] attribute better directs to where data will be passed from 
        public bool CreateUser([FromBody] UserDTO newUser)
        {
            return _userServices.CreateUser(newUser);
            //above statement is being EVALUATED as a boolean, returns true or false
        }

        [HttpPost]
        [Route("Login")]

        public IActionResult Login([FromBody] UserDTO user)
        {
            string stringToken = _userServices.Login(user);
            if(stringToken != null)
            {
                return Ok(new {Token = stringToken});
            }else
            {
                return Unauthorized(new {Message = "Login was unsuccessful. Invalid email or password."});
            }
        }

        [Authorize]
        [HttpGet]
        [Route("AuthenticUser")]
        public string AuthenticUserCheck(){
            return "You are logged in and allowed to be here.";
        }

        [HttpPut]
        [Route("UpdatePassword")]

        public bool UpdatePassword([FromBody] UserDTO user)
        {
            return _userServices.UpdatePassword(user);
        }

        [HttpDelete]
        [Route("DeleteAccount")]

        public bool DeleteAccount([FromBody] UserDTO user)
        {
            return _userServices.DeleteAccount(user);
        }
    }
}