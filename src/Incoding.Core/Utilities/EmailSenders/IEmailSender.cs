using System;
using System.Net.Mail;

namespace Incoding.Core.Utilities.EmailSenders
{
    #region << Using >>

    #endregion

    [Obsolete("Please use SendEmailCommand")]
    public interface IEmailSender : IDisposable
    {
        void Send(MailMessage mailMessage);
    }
}