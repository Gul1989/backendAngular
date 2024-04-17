using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Register.Models;
using System;
using System.Linq;

namespace Register.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserContext _context;

        public UserController(IConfiguration config, UserContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public IActionResult Create(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return Conflict("User already exists.");
            }

            user.MemberSince = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Firstname = updatedUser.Firstname;
            user.Lastname = updatedUser.Lastname;
            user.Email = updatedUser.Email;
            user.Mobile = updatedUser.Mobile;
            user.Gender = updatedUser.Gender;
            user.MemberSince = updatedUser.MemberSince;

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}