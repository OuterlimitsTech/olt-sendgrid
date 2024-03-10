using System.Collections.Generic;

namespace OLT.Email.SendGrid.Tests.Assets
{
    public class FakeJsonEmailTemplate : JsonEmailTemplate
    {
        public FakeJsonEmailTemplate() : base(nameof(FakeJsonEmailTemplate))
        {

        }    

        public static FakeJsonEmailTemplate FakerData(int numTo, int numCarbonCopy)
        {
            var result = new FakeJsonEmailTemplate
            {
                TemplateData = EmailDataJson.FakerData(),
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