namespace OLT.Email.SendGrid.Tests.Assets
{
    public class FakeNoTemplateDataTemplate : IOltEmailTemplateId, IOltEmailTemplate
    {
        public string TemplateId { get; set; } = nameof(FakeNoTemplateDataTemplate);

        public OltEmailRecipients Recipients { get; set; } = new OltEmailRecipients();

        public object GetTemplateData()
        {
            return null;
        }

        public static FakeNoTemplateDataTemplate FakerData(int numTo, int numCarbonCopy)
        {
            var result = new FakeNoTemplateDataTemplate();         

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
