using System.Collections.Generic;
using OLT.Core;
using OLT.Email;

namespace OLT.Email.SendGrid.Tests.Assets
{

    public class JsonEmailTemplate : OltEmailJsonTemplate<EmailDataJson>
    {
        public JsonEmailTemplate(string templateId)
        {
            TemplateId = templateId;
        }

        public override string TemplateId { get; set; }
        
        public override EmailDataJson TemplateData { get; set; }

        public static JsonEmailTemplate FakerData(string templateId)
        {
            return new JsonEmailTemplate(templateId)
            {                
                Recipients = new OltEmailRecipients
                {
                   To = new List<IOltEmailAddress>
                   {
                       new OltEmailAddress(Faker.Internet.Email(), Faker.Name.First()),
                       new OltEmailAddress(Faker.Internet.Email(), Faker.Name.First()),
                   }
                },
                TemplateData = EmailDataJson.FakerData(),
            };
        }

    }
}