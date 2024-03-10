using System.Linq;

namespace OLT.Email.SendGrid.Tests.Assets
{

    public class EmailDataJson
    {        
        public EmailRecipientJson Recipient { get; set; } = new EmailRecipientJson();
        public EmailDataCommunicationJson Communication { get; set; } = new EmailDataCommunicationJson();
        public EmailDataBuildVersionJson Build { get; set; } = new EmailDataBuildVersionJson();

        public static EmailDataJson FakerData()
        {
            return new EmailDataJson
            {
                Recipient = new EmailRecipientJson
                {
                    First = Faker.Name.First(),
                    FullName = Faker.Name.FullName()
                },
                Build = new EmailDataBuildVersionJson
                {
                    Version = Faker.Country.Name()
                },
                Communication = new EmailDataCommunicationJson
                {
                    Body1 = Faker.Lorem.Sentences(4).LastOrDefault(),
                    Body2 = Faker.Lorem.Sentences(10).LastOrDefault(),
                }
            };
        }
    }
}