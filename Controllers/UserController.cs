using Microsoft.AspNetCore.Mvc;
using Task.Interfaces;
using User.Models;
using Task.Models;
using System.Security.Claims;
using Task.Services;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.Json;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace User.Controllers
{
    [ApiController]
    // [Route("/user")]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private IUserService UserService;
         private ITokenService VTokenService;
        private static user CurrentUser;
        public UserController(IUserService UserService)
        {
            this.UserService = UserService;
        }
        [HttpGet]
        public ActionResult<List<user>> GetAll() =>
              UserService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<user> Get(int id)
        {
            var myUser = UserService.Get(id);

            if (myUser == null)
                return NotFound();

            return myUser;
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] user User)
        {
                 var dt = DateTime.Now;

            if (User.Name != "Admin" || User.Password != "123")
            {
                CurrentUser = UserService.IsExist(User);
                if(CurrentUser == null)
                {
                    return Unauthorized();
                }
                var claims = new List<Claim> {
                    new Claim("type", "User"),
                    new Claim("Id", CurrentUser.Id.ToString())
                };

                var token = VTokenService.GetToken(claims);

                return new OkObjectResult(VTokenService.WriteToken(token));                      
            }

            else
            {
                var claims = new List<Claim> {
                    new Claim("type", "Admin"),
                    new Claim("Id", "0") 
                };

                CurrentUser = new user();
                CurrentUser.Id = 0;
                CurrentUser.Name = "Admin";
                CurrentUser.Password = "123";

                var token = VTokenService.GetToken(claims);

                return new OkObjectResult(VTokenService.WriteToken(token));
            }
            // var claims = new List<Claim>();
            // user login = UserService.Get(User.Id);
            // if (login.IsAdmin)
            // {
            //     claims.Add(
            //         new Claim("type", "Admin")
            //     );
            // }
            // else
            // {
            //     claims.Add(
            //         new Claim("type", "User")
            //     );
            // }
            // var token = TokenService.GetToken(claims);
            // return new OkObjectResult(TokenService.WriteToken(token));

            // var dt = DateTime.Now;

            // if (User.Username != "Wray"
            // || User.Password != $"W{dt.Year}#{dt.Day}!")
            // {
            //     return Unauthorized();
            // }

            // var claims = new List<Claim>
            // {
            //     new Claim("type", "Admin"),
            // };

            // var token = TokenService.GetToken(claims);

            // return new OkObjectResult(FbiTokenService.WriteToken(token));
        }
        [HttpPost]
        public IActionResult Create(user myUser)
        {
            UserService.Add(myUser);
            return CreatedAtAction(nameof(Create), new { id = myUser.Id }, myUser);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var myUser = UserService.Get(id);
            if (myUser is null)
                return NotFound();

            UserService.Delete(id);

            return Content(UserService.Count.ToString());
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, user myUser)
        {
            if (id != myUser.Id)
                return BadRequest();

            var existinguser = UserService.Get(id);
            if (existinguser is null)
                return NotFound();

            UserService.Update(myUser);

            return NoContent();
        }
    }
}