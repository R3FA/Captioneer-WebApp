using API.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            this._emailService = emailService;
        }


        [HttpPost("2StepVerificationMail")]
        public IActionResult TwoStepVerificationMail(EmailViewModel req)
        {
            this._emailService.TwoStepVerificationMail(req);
            return Ok();
        }
    }
}
