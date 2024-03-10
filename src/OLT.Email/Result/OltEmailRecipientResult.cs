using System;
using System.Collections.Generic;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public class OltEmailRecipientResult
    {
        public virtual List<OltEmailAddressResult> To { get; set; } = new List<OltEmailAddressResult>();
        public virtual List<OltEmailAddressResult> CarbonCopy { get; set; } = new List<OltEmailAddressResult>();
    }
}