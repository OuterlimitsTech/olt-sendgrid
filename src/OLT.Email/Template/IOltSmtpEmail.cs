using System.Collections.Generic;

namespace OLT.Email
{
    public interface IOltSmtpEmail 
    {
        string Subject { get; }
        string Body { get; }
        OltEmailAddress From { get; }
        OltEmailRecipients Recipients { get; }
    }
}