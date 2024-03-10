namespace OLT.Email.SendGrid
{
    public interface IOltEmailTemplate
    {
        OltEmailRecipients Recipients { get; }
    }

}