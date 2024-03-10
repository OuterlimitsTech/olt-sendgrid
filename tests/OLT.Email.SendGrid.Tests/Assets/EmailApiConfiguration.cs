using Microsoft.Extensions.Options;
using OLT.Email;
using OLT.Email.SendGrid;
using System.Collections.Generic;

namespace OLT.Email.SendGrid.Tests.Assets
{
    public class SendGridProductionConfiguration : OltEmailConfigurationSendGrid
    {
        public string TemplateIdJson { get; set; }
        public string TemplateIdTag { get; set; }
        public string TemplateIdNoData { get; set; }
        public string ToEmail { get; set; }
        public string RunNumber { get; set; }
        public int? UnsubscribeGroupId { get; set; }
    }

}