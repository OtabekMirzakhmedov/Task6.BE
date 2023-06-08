using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Task6.BE
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly MailDbContext _dbContext;

        public EmailController(MailDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("receivers")]
        public async Task<IActionResult> GetReceivers([FromQuery] string username)
        {
            var receivers = await _dbContext.Emails
                .Where(e => e.SenderName == username)
                .Select(e => e.ReceiverName)
                .Distinct()
                .ToListAsync();

            return Ok(receivers);
        }
    }
}
