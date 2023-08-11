namespace GarageBuddy.Services.Messaging.Contracts
{
    using System.Threading.Tasks;

    using Common.Core.Wrapper;

    public interface IEmailService
    {
        Task<IResult> SendResetPasswordEmail(string userEmail, string htmlContent);
    }
}
