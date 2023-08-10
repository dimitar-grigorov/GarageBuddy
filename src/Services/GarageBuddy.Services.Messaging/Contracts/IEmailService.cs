namespace GarageBuddy.Services.Messaging.Contracts
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendResetPasswordEmail(string userEmail, string htmlContent);
    }
}
