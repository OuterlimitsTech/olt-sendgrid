using System;
using System.Collections.Generic;

namespace OLT.Email.SendGrid
{    
    public abstract class OltCustomArgsArgs<T> : OltUnsubscribeGroupArgs<T>
    where T : OltCustomArgsArgs<T>
    {
        protected Dictionary<string, string> CustomArgs { get; set; } = new Dictionary<string, string>();

        protected OltCustomArgsArgs()
        {
        }

        /// <summary>
        /// Adds Custom Arg to Email Request that can be used to assoicate to internal data when subscribing to the Webhooks
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.SendGrid")]
        public T WithCustomArg(string customArgKey, string customArgValue)
        {
            if (customArgKey == null)
            {
                throw new ArgumentNullException(nameof(customArgKey));
            }
            if (customArgValue == null)
            {
                throw new ArgumentNullException(nameof(customArgValue));
            }
            this.CustomArgs.Add(customArgKey, customArgValue);
            return (T)this;
        }
    }
}
