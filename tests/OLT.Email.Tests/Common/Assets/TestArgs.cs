using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.Tests.Common.Assets
{
    [Obsolete]
    public class TestArgs : OltFromEmailArgs<TestArgs>
    {

        public OltEmailAddress EmailValue => base.From;
        public List<OltEmailAddress> ToValue => base.To;
        public List<OltEmailAddress> CarbonCopyValue => base.CarbonCopy;
        public OltEmailRecipientResult BuildRecipientsValue => base.BuildRecipients();
        public List<OltEmailAttachment> AttachmentValue => base.Attachments;

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
