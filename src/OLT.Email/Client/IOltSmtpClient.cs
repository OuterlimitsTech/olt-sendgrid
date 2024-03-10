using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email
{
    public interface IOltSmtpClient : IOltEmailClient<SmtpClient, MailMessage, OltEmailResult>
    {

    }
}
