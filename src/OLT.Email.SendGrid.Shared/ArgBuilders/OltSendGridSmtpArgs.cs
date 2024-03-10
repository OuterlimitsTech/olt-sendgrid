#nullable disable

using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OLT.Email.SendGrid
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public abstract class OltSendGridSmtpArgs<T> : OltSmtpNetworkCredentialArgs<T>, IOltSmtpClient, IOltApiKeyArgs<T>
        where T : OltSendGridSmtpArgs<T>
    {
        protected string ApiKey { get; set; }
        
        protected OltSendGridSmtpArgs()
        {

        }

        /// <summary>
        /// SendGrid API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        public override SmtpClient CreateClient()
        {
            var smtpServer = new OltSmtpServerSendGrid(ApiKey);

            base.WithSmtpHost(smtpServer.Host);
            base.WithSmtpPort(smtpServer.Port ?? 587);
            base.WithSmtpNetworkCredentials(smtpServer.Credentials.Username, smtpServer.Credentials.Password);
            SmtpSSLDisabled = false;

            return base.CreateClient();
        }     
    }
}
