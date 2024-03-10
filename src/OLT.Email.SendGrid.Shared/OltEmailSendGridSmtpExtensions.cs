using System;
using System.Collections.Generic;
using System.Text;

namespace OLT.Email.SendGrid.Common
{
    public static class OltEmailSendGridSmtpExtensions
    {
        /// <summary>
        /// Sends email with exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="apiKey"></param>
        /// <param name="template"></param>
        /// <param name="rethrowException"></param>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of jcamp.FluentEmail.Smtp")]
        public static void OltEmailError(this Exception ex, string apiKey, IOltApplicationErrorEmail template, bool rethrowException = false)
        {
            OltSmtpEmailExtensions.OltEmailError(ex, new OltSmtpServerSendGrid(apiKey), template, rethrowException);
        }

    }
}
