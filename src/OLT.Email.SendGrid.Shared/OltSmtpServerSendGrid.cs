using System;

namespace OLT.Email.SendGrid
{
    [Obsolete("OLT.Email.SendGrid is being deprecated in favor of jcamp.FluentEmail.Smtp")]
    public class OltSmtpServerSendGrid : OltSmtpServer
    {        
        protected OltSmtpServerSendGrid()
        {
            Host = "smtp.sendgrid.net";
            Port = 587;
            Credentials.Username = "apikey";            
        }

        public OltSmtpServerSendGrid(string apiKey) : this()
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            Credentials.Password = apiKey;
        }
    }
}
