using System.Collections.Generic;

namespace OLT.Email
{
    public class OltSmtpEmail : IOltSmtpEmail
    {
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual OltEmailAddress From { get; set; } = new OltEmailAddress();
        public virtual OltEmailRecipients Recipients { get; set; } = new OltEmailRecipients();
    }
}