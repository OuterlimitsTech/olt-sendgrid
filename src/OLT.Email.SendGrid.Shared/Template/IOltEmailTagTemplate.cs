using System.Collections.Generic;

namespace OLT.Email.SendGrid
{
    public interface IOltEmailTagTemplate : IOltEmailTemplateId, IOltEmailTemplate
    {        
        List<OltEmailTag> Tags { get; }
    }

}