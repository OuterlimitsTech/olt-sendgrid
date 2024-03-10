#nullable disable
using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public abstract class OltBodyArgs<T> : OltSubjectLineArgs<T>
        where T : OltBodyArgs<T>
    {
        protected  string Body { get; set; }

        protected OltBodyArgs()
        {
        }

        /// <summary>
        /// Email Body
        /// </summary>
        /// <returns></returns>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public T WithBody(string body)
        {
            this.Body = body ?? throw new ArgumentNullException(nameof(body));
            return (T)this;
        }

        /// <summary>
        /// Sets email body and subject to format an error
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method is being deprecated in favor of OLT.FluentEmail.Extensions -> OltAppError")]
        public T WithAppError(Exception exception, string appName, string environment)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            if (appName == null)
            {
                throw new ArgumentNullException(nameof(appName));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            this.SubjectLine = $"[{appName}] APPLICATION ERROR in {environment} Environment occurred at {DateTimeOffset.Now:f}";
            this.Body = $"The following error occurred:{Environment.NewLine}{exception}";
            return (T)this;
        }

        public override List<string> ValidationErrors()
        {
            var errors = base.ValidationErrors();
            if (string.IsNullOrWhiteSpace(Body))
            {
                errors.Add(OltSmtpArgErrors.Body);
            }
            return errors;
        }

        public override MailMessage CreateMessage(OltEmailRecipientResult recipients)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(From.Email, From.Name),
                Subject = SubjectLine,
                Body = Body,
            };

            ConfigureRecipients(msg, recipients);

            return msg;
        }
    }
}
