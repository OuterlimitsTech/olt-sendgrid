using System.Collections.Generic;

namespace OLT.Email.SendGrid
{
    public abstract class OltEmailJsonTemplate<TModel> : IOltEmailJsonTemplate<TModel>
        where TModel : class

    {
        public abstract string TemplateId { get; set; }
        
        public abstract TModel TemplateData { get; set; }

        public virtual OltEmailRecipients Recipients {  get; set; } = new OltEmailRecipients();

        public object GetTemplateData()
        {
            return TemplateData;
        }
    }
}