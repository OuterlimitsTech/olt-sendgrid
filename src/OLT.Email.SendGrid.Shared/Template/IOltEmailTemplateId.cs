namespace OLT.Email.SendGrid
{
    public interface IOltEmailTemplateId
    {
        string TemplateId { get; }
        object GetTemplateData();
    }

}