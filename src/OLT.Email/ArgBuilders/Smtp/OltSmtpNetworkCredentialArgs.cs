using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace OLT.Email
{
    public abstract class OltSmtpNetworkCredentialArgs<T> : OltSmtpSendArgs<T>
        where T : OltSmtpNetworkCredentialArgs<T>
    {        
        protected  string SmtpUsername { get; set; }
        protected  string SmtpPassword { get; set; }

        protected OltSmtpNetworkCredentialArgs()
        {
        }

        /// <summary>
        /// SMTP Server Username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public T WithSmtpNetworkCredentials(string username, string password)
        {
            this.SmtpUsername = username ?? throw new ArgumentNullException(nameof(username));
            this.SmtpPassword = password ?? throw new ArgumentNullException(nameof(password));
            return (T)this;
        }


        public override SmtpClient CreateClient()
        {
            var client = base.CreateClient();
            if (!string.IsNullOrWhiteSpace(SmtpUsername) || !string.IsNullOrWhiteSpace(SmtpPassword))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            }
            return client;
        }
    }


}
