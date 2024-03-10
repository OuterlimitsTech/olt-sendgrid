using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.SendGrid.Tests.Assets
{
    public class SendGridTemplateTestArgs : OltTemplateArgs<SendGridTemplateTestArgs>
    {        
        public string TemplateIdValue => base.TemplateId;
        public object TemplateDataValue => base.TemplateData;
        public string ApiKeyValue => base.ApiKey;
        public Dictionary<string, string> CustomArgsValue => base.CustomArgs;
        public int? UnsubscribeGroupIdValue => base.UnsubscribeGroupId;
        public bool ClickTrackingValue => base.ClickTracking;
        public bool SandboxModeValue => base.SandboxMode;


        public bool DoValidation()
        {
            var errors = ValidationErrors();
            if (errors.Any())
            {
                throw new OltSendGridValidationException(errors);
            }
            return true;
        }      

    }
}
