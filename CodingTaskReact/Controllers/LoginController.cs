using Cleverbit.CodingTask.Data;
using Cleverbit.CodingTask.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodingTaskReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IHashService hashService;
        private readonly CodingTaskContext context;
        public LoginController(IHashService hashService, CodingTaskContext context)
        {
            this.hashService = hashService;
            this.context = context;
        }

        public class Credentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            var hashPassword = await hashService.HashText(credentials.Password);
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == credentials.Username && u.Password == hashPassword);
            if (user == null)
                return Unauthorized();
            else
                return Ok();
        }
    }
}
