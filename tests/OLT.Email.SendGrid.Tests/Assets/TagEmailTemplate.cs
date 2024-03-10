using System.Collections.Generic;
using System.Linq;

namespace OLT.Email.SendGrid.Tests.Assets
{
    public class TagEmailTemplate : OltEmailTagTemplate
    {
        public TagEmailTemplate(string templateId)
        {
            TemplateId = templateId;
        }

        public override string TemplateId { get; set; }

        public EmailRecipientJson Recipient { get; set; } = new EmailRecipientJson();
        public EmailDataBuildVersionJson Build { get; set; }  = new EmailDataBuildVersionJson();
        
        public string Body1 { get; set; }
        public string Body2 { get; set; }

        public override List<OltEmailTag> Tags => new List<OltEmailTag>
        {
            new OltEmailTag { Tag = "First", Value = Recipient.First },
            new OltEmailTag { Tag = "BuildVersion", Value = Build.Info },
            new OltEmailTag { Tag = nameof(Body1), Value = Body1 },
            new OltEmailTag { Tag = nameof(Body2), Value = Body2 },
        };

        public static TagEmailTemplate FakerData(string templateId)
        {
            return new TagEmailTemplate(templateId)
            {
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                   {
                       new OltEmailAddress(Faker.Internet.Email(), Faker.Name.First()),
                       new OltEmailAddress(Faker.Internet.Email(), Faker.Name.First()),
                   }
                },
                Recipient = new EmailRecipientJson
                {
                    First = Faker.Name.First(),
                    FullName = Faker.Name.FullName()
                },
                Build = new EmailDataBuildVersionJson
                {
                    Version = Faker.Country.Name()
                },
                Body1 = Faker.Lorem.Sentences(4).LastOrDefault(),
                Body2 = Faker.Lorem.Sentences(10).LastOrDefault(),
            };
        }
    }
}