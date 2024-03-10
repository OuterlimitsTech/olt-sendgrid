using System.Collections.Generic;

namespace OLT.Email
{
    public interface IOltApplicationErrorEmail : IOltSmtpEmail
    {
        string AppName { get; } 
        string Environment { get; }
    }
}