namespace API.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailViewModel req);
    }
}
