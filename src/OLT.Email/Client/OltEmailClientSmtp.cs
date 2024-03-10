using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public class OltEmailClientSmtp : OltSmtpNetworkCredentialArgs<OltEmailClientSmtp>
    {
       
    }
}
