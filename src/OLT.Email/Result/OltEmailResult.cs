using System;
using System.Collections.Generic;
using System.Linq;
using OLT.Core;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public class OltEmailResult : IOltEmailResult
    {
        public virtual bool Success => !Errors.Any();
        public virtual List<string> Errors { get; set; } = new List<string>();
        public virtual OltEmailRecipientResult RecipientResults { get; set; } = new OltEmailRecipientResult();
    }
}
