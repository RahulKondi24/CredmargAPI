using CredmargAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using CredmargAPI.Repository;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace CredmargAPI.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly SmtpSettings _smtpSettings;

        private readonly CredmargRepository _repository;

        public EmailController(CredmargRepository repository, IOptions<SmtpSettings> smtpSettings)
        {
            _repository = repository;
            _smtpSettings = smtpSettings.Value;
        }

        [HttpPost("send-emails")]
        [Authorize(Roles = "Admin")]
        public IActionResult SendEmail([FromBody] List<string> vendorEmails)
        {
            using (var mailClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                mailClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                mailClient.EnableSsl = _smtpSettings.EnableSsl;

                foreach (var email in vendorEmails)
                {
                    var vendor = _repository.Vendors.FirstOrDefault(v => v.Email == email);
                    if (vendor != null)
                    {
                        var body = $"Sending payments to vendor {vendor.Name} at upi {vendor.UPI}";
                        var message = new MailMessage
                        {
                            From = new MailAddress(_smtpSettings.Username),
                            To = { new MailAddress(vendor.Email) },
                            Subject = "Payment Notification",
                            Body = body
                        };

                        try
                        {
                            mailClient.Send(message);
                            _repository.SentEmails.Add(new EmailMessage
                            {
                                To = vendor.Email,
                                Subject = "Payment Notification",
                                Body = body
                            });
                            Console.WriteLine($"Email sent to {vendor.Email}: {body}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending email to {vendor.Email}: {ex.Message}");
                        }
                    }
                }
            }

            return Ok();
        }

        [HttpGet("getall-emails")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetSentEmails()
        {
            var emails = _repository.Vendors.Select(v => v.Email);
            return Ok(emails);
        }
    }
}
