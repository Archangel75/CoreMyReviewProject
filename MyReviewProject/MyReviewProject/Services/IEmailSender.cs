using System.Threading.Tasks;

namespace MyReviewProject.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}