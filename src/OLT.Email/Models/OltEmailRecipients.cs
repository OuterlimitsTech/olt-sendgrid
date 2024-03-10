using System.Collections.Generic;

namespace OLT.Email
{
    public class OltEmailRecipients
    {
        public virtual List<IOltEmailAddress> To { get; set; } = new List<IOltEmailAddress>();
        public virtual List<IOltEmailAddress> CarbonCopy { get; set; } = new List<IOltEmailAddress>();
    }
}