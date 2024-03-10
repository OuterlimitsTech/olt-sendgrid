using System;
using System.Linq;
using System.Threading.Tasks;

namespace OLT.Email
{
    public abstract class OltSmtpSendArgs<T> : OltCalendarInviteArgs<T>
      where T : OltSmtpSendArgs<T>
    {
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public override async Task<OltEmailResult> SendAsync(bool throwExceptions)
        {

            var result = new OltEmailResult();

            try
            {
                var errors = ValidationErrors();
                if (errors.Any())
                {
                    throw new OltEmailValidationException(errors);
                }

                result.RecipientResults = BuildRecipients();

                using (var client = CreateClient())
                {
                    using (var msg = CreateMessage(result.RecipientResults))
                    {
                        if (msg.To.Any())
                        {
                            await client.SendMailAsync(msg);
                        }
                        else
                        {
                            throw new OltEmailNoRecipientsValidationException();
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                {
                    throw;
                }
                result.Errors.Add(ex.ToString());
            }

            return result;
        }
    }
}
