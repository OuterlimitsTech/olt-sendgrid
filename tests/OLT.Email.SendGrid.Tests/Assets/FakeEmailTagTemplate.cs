using OLT.Email.SendGrid;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Email.SendGrid.Tests.Assets
{
    public class FakeEmailTagTemplate : OltEmailTagTemplate
    {
        public override string TemplateId { get; set; } = nameof(FakeEmailTagTemplate);

        public string Value1 { get; set; }
        public string Value2 { get; set; }

        public override List<OltEmailTag> Tags => new List<OltEmailTag>
        {
            new OltEmailTag(nameof(Value1), Value1),
            new OltEmailTag(nameof(Value2), Value2),
        };

        public static FakeEmailTagTemplate FakerData(int numTo, int numCarbonCopy)
        {
            var result = new FakeEmailTagTemplate
            {                
                Value1 = Faker.Lorem.Words(20).Last(),
                Value2 = Faker.Lorem.Words(10).Last(),               
            };

            for (int i = 0; i < numTo; i++)
            {
                result.Recipients.To.Add(SendGridHelper.FakerEmailAddress());
            }

            for (int i = 0; i < numCarbonCopy; i++)
            {
                result.Recipients.CarbonCopy.Add(SendGridHelper.FakerEmailAddress());
            }

            return result;
        }
    }
}
