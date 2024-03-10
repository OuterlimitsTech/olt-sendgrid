using System;

namespace OLT.Email
{
    public abstract class OltSmtpDisableSslArgs<T> : OltSmtpHostArgs<T>
      where T : OltSmtpDisableSslArgs<T>
    {
        protected  bool SmtpSSLDisabled { get; set; }

        protected OltSmtpDisableSslArgs()
        {
        }

        /// <summary>
        /// Disabled SSL for SMTP
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public T WithSmtpSSLDisabled(bool value)
        {
            this.SmtpSSLDisabled = value;
            return (T)this;
        }

    }

   
}
