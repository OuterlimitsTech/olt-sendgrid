using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.Tests.Smtp.Assets
{
    public class SmtpTestArgs : OltSmtpNetworkCredentialArgs<SmtpTestArgs>
    {
        public string SmtpHostValue => base.SmtpHost;
        public bool SmtpSSLDisabledValue => base.SmtpSSLDisabled;
        public int SmtpPortValue => base.SmtpPort;
        public string BodyValue => base.Body;
        public string SubjectLineValue => base.SubjectLine;
        public string SmtpUsernameValue => base.SmtpUsername;
        public string SmtpPasswordValue => base.SmtpPassword;

        public bool DoValidation()
        {
            var errors = ValidationErrors();
            if (errors.Any())
            {
                throw new OltEmailValidationException(errors);
            }
            return true;
        }       
    }
}
