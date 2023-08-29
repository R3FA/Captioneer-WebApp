namespace API.Services.EmailService
{
    public interface IEmailService
    {
        void TwoStepVerificationMail(EmailViewModel req);
    }
}