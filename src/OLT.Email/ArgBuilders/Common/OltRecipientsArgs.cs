using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public abstract class OltRecipientsArgs<T> : OltEmailArgsWhitelist<T>
      where T : OltRecipientsArgs<T>
    {
        protected  List<OltEmailAddress> To { get; set; } = new List<OltEmailAddress>();
        protected  List<OltEmailAddress> CarbonCopy { get; set; } = new List<OltEmailAddress>();

        protected OltRecipientsArgs()
        {
        }

        /// <summary>
        /// Recipients to send to
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public T WithRecipients(OltEmailRecipients value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            value.To?.ToList().ForEach(rec =>
            {
                To.Add(new OltEmailAddress(rec.Email, rec.Name));
            });

            value.CarbonCopy?.ToList().ForEach(rec =>
            {
                CarbonCopy.Add(new OltEmailAddress(rec.Email, rec.Name));
            });

            return (T)this;
        }

        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public override OltEmailRecipientResult BuildRecipients()
        {
            var recipientResult = new OltEmailRecipientResult();

            To.ForEach(rec =>
            {
                if (!recipientResult.To.Any(x => x.Email.Equals(rec.Email, System.StringComparison.OrdinalIgnoreCase)))
                {
                    recipientResult.To.Add(new OltEmailAddressResult(rec, this));
                }
            });

            CarbonCopy.ForEach(rec =>
            {
                if (!recipientResult.To.Any(x => x.Email.Equals(rec.Email, System.StringComparison.OrdinalIgnoreCase)) &&
                    !recipientResult.CarbonCopy.Any(x => x.Email.Equals(rec.Email, System.StringComparison.OrdinalIgnoreCase)))
                {
                    recipientResult.CarbonCopy.Add(new OltEmailAddressResult(rec, this));
                }
            });

            return recipientResult;
        }

        public override List<string> ValidationErrors()
        {
            var errors = base.ValidationErrors();
            if (!To.Any())
            {
                errors.Add(OltEmailErrors.Recipients);
            }
            return errors;
        }
    }

}
