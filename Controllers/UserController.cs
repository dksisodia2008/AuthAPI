using AuthAPI.API.Data;
using AuthAPI.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace AuthAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private AppDbContext _appDbContext;
        public UserController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            
            if (string.IsNullOrEmpty(userObj.UserName))
            {
                return BadRequest();
            }
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == userObj.UserName && u.Password == userObj.Password);

            if (user == null)
            {
                return NotFound(new { Message= "User Not Found!" });
            }
            return Ok(new { Message = "Login Successfully!" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Registration([FromBody] User userObj)
        {

            if (string.IsNullOrEmpty(userObj.UserName))
            {
                return BadRequest();
            }
            await _appDbContext.Users.AddAsync(userObj);
            await _appDbContext.SaveChangesAsync();
            return Ok(new { Message = "Record Save Successfully!" });
        }
    }
}
