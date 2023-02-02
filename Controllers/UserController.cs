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
    [Route("User")]
    public class UserController : ControllerBase
    {
        private IUserService UserService;
        // private ITokenService VTokenService;
        private static user CurrentUser;
         
        public UserController(IUserService UserService)
        {
            this.UserService = UserService;
    
        }
        [HttpGet]
        [Authorize(Policy = "Admin")]
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
             var claims = new List<Claim>();
                 var dt = DateTime.Now;

            if (User.Name != "Admin" || User.Password != "123")
            {
                CurrentUser = UserService.IsExist(User);
                if(CurrentUser == null)
                {
                    return Unauthorized();
                }
                else{
                  
                    claims.Add(new Claim("type", "User"));
                    claims.Add( new Claim("Id", CurrentUser.Id.ToString()));
                   
                   
               
               
                };                     
            }

            else
            {
               
                   claims.Add( new Claim("type", "Admin"));
                   claims.Add( new Claim("Id", "0") );

                CurrentUser = new user();
                CurrentUser.Id = 0;
                CurrentUser.Name = "Admin";
                CurrentUser.Password = "123";

               
            }
             var token = TokenService.GetToken(claims);
             CurrentUser=User;
                return new OkObjectResult(TokenService.WriteToken(token));
          
        }
        [HttpPost]

        [Authorize(Policy = "Admin")]
        public IActionResult Create(user myUser)
        {
            UserService.Add(myUser);
            return CreatedAtAction(nameof(Create), new { id = myUser.Id }, myUser);

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var myUser = UserService.Get(id);
            if (myUser is null)
                return NotFound();

            UserService.Delete(id);

            return Content(UserService.Count.ToString());
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
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