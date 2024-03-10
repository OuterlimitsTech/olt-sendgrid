using System.Collections.Generic;
using System.Linq;

namespace OLT.Email.SendGrid
{
    public abstract class OltEmailTagTemplate : IOltEmailTagTemplate
    {
        public abstract string TemplateId { get; set; }
        public abstract List<OltEmailTag> Tags { get; }
        public OltEmailRecipients Recipients { get; set; } = new OltEmailRecipients();

        public object GetTemplateData()
        {
            return OltEmailTag.ToDictionary(Tags.ToList());
        }
    }

}