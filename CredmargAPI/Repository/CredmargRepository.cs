using CredmargAPI.Models;

namespace CredmargAPI.Repository
{
    public class CredmargRepository
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Vendor> Vendors { get; set; } = new List<Vendor>();
        public List<EmailMessage> SentEmails { get; set; } = new List<EmailMessage>();
    }

}
