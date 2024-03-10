namespace OLT.Email.SendGrid
{
    public interface IOltEmailJsonTemplate<out TModel> : IOltEmailTemplateId, IOltEmailTemplate
        where TModel : class
    {
        TModel TemplateData { get; }
    }

}