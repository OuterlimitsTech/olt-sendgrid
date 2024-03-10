using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public abstract class OltEmailBuilderArgs : IOltEmailClient 
    {
        
        // <summary>
        // Production Enabled
        // </summary>
        protected abstract bool Enabled { get; }

        // <summary>
        // Determines if Email can be sent depending on whitelist or production
        // </summary>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public abstract bool AllowSend(string emailAddress);

        public virtual bool IsValid => !ValidationErrors().Any();

        public virtual List<string> ValidationErrors()
        {
            return new List<string>();
        }

        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public abstract OltEmailRecipientResult BuildRecipients();        
        
    }
}
