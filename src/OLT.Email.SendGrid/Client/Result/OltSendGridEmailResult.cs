namespace OLT.Email.SendGrid
{
    public class OltSendGridEmailResult : OltEmailResult
    {
        public OltSendGridResponseJson SendGrid { get; set; } = new OltSendGridResponseJson();
    }
}
