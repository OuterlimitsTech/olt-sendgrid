using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OLT.Constants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OLT.Email.SendGrid
{
    
    public abstract class OltApiKeyArgs<T> : OltFromEmailArgs<T>, IOltApiKeyArgs<T>, IOltEmailClient<SendGridClient, SendGridMessage, OltSendGridEmailResult>
        where T : OltApiKeyArgs<T>
    {
        protected string ApiKey { get; set; }

        protected OltApiKeyArgs()
        {
        }

        /// <summary>
        /// SendGrid API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.SendGrid")]
        public T WithApiKey(string apiKey)
        {
            this.ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            return (T)this;
        }

        public override List<string> ValidationErrors()
        {
            var errors = base.ValidationErrors();
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                errors.Add(OltArgErrorsSendGrid.ApiKey);
            }
            return errors;
        }

        public SendGridClient CreateClient()
        {
            return new SendGridClient(ApiKey);
        }

        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.SendGrid")]
        public abstract SendGridMessage CreateMessage(OltEmailRecipientResult recipients);


        public virtual void ConfigureRecipients(SendGridMessage msg, OltEmailRecipientResult recipients)
        {
            recipients.To.Where(p => !p.Skipped).ToList().ForEach(rec =>
            {
                msg.AddTo(new EmailAddress(rec.Email, rec.Name));
            });

            recipients.CarbonCopy.Where(p => !p.Skipped).ToList().ForEach(rec =>
            {
                msg.AddCc(new EmailAddress(rec.Email, rec.Name));
            });

        }

        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.SendGrid")]
        public virtual OltSendGridEmailResult Send(bool throwExceptions)
        {
            try
            {
                return Task.Run(() => SendAsync(throwExceptions)).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.SendGrid")]
        public abstract Task<OltSendGridEmailResult> SendAsync(bool throwExceptions);

    }
}
