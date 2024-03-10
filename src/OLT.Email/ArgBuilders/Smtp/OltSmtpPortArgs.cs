using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace OLT.Email
{
    public abstract class OltSmtpPortArgs<T> : OltSmtpDisableSslArgs<T>
        where T : OltSmtpPortArgs<T>
    {
        protected  int SmtpPort { get; set; } = 587;

        protected OltSmtpPortArgs()
        {
        }

        /// <summary>
        /// SMTP Server Port
        /// </summary>
        /// <remarks>Default is 587</remarks>
        /// <param name="port"></param>
        /// <returns></returns>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public T WithSmtpPort(int port)
        {
            if (port <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(port), "must be greater than zero");
            }
            this.SmtpPort = port;
            return (T)this;
        }

        public override SmtpClient CreateClient()
        {
            var client = new SmtpClient(SmtpHost, SmtpPort);
            if (!SmtpSSLDisabled)
            {
                client.EnableSsl = true;
            }         
            return client;
        }
    }

   
}
