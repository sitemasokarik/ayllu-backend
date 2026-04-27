using DcodePe.Catering.Domain.Models.MailerSendEmail;

namespace DcodePe.Catering.Application.External.MailerSendEmail
{
    public interface IMailerSendEmailService
    {
        Task<bool> Execute(MailerSendEmailRequestModel model);
    }
}
