using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Email
{
    public abstract class OltEmailArgsWhitelist<T> : OltEmailArgsProduction<T>
        where T : OltEmailArgsWhitelist<T>
    {
        protected List<string> DomainWhitelist { get; set; } = new List<string>();
        protected List<string> EmailWhitelist { get; set; } = new List<string>();

        protected OltEmailArgsWhitelist()
        {
        }

        /// <summary>
        /// Adds emails and domains to whitelist
        /// </summary>
        /// <param name="config"><see cref="OltEmailConfigurationWhitelist"/></param>
        /// <returns><typeparamref name="T"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.Extensions")]
        public T WithWhitelist(OltEmailConfigurationWhitelist config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (!string.IsNullOrWhiteSpace(config.Domain))
            {
                this.DomainWhitelist.AddRange(config.DomainParsed.Except(this.DomainWhitelist));
            }
            
            if (!string.IsNullOrWhiteSpace(config.Email))
            {
                this.EmailWhitelist.AddRange(config.EmailParsed.Except(this.EmailWhitelist));
            }
            
            return (T)this;
        }

        /// <summary>
        /// Adds email address to whitelist
        /// </summary>
        /// <param name="emailAddress"><see cref="IOltEmailAddress"/></param>
        /// <returns><typeparamref name="T"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">When email is null</exception>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.Extensions")]
        public T WithWhitelist(IOltEmailAddress emailAddress)
        {
            if (emailAddress == null)
            {
                throw new ArgumentNullException(nameof(emailAddress));
            }

            if (emailAddress.Email == null)
            {
                throw new InvalidOperationException($"{nameof(emailAddress)}.{nameof(emailAddress.Email)} is null");
            }

            if (!this.EmailWhitelist.Any(value => string.Equals(emailAddress.Email, value, StringComparison.OrdinalIgnoreCase)))
            {
                this.EmailWhitelist.Add(emailAddress.Email);
            }

            return (T)this;
        }

        /// <summary>
        /// Determines if Email can be sent depending on EnableProductionEnvironment() is true or <see cref="OltEmailConfigurationWhitelist"/> and EnableProductionEnvironment() is false
        /// </summary>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.Extensions")]
        public override bool AllowSend(string emailAddress)
        {
            if (base.Enabled)
            {
                return true;
            }

            return DomainWhitelist.Any(p => emailAddress.EndsWith(p, StringComparison.OrdinalIgnoreCase)) ||
                   EmailWhitelist.Any(p => emailAddress.Equals(p, StringComparison.OrdinalIgnoreCase));
        }

    }

}
