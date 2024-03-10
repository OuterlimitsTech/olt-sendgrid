using System;

namespace OLT.Email.SendGrid
{
    public abstract class OltEnableSandboxArgs<T> : OltCustomArgsArgs<T>
        where T : OltEnableSandboxArgs<T>
    {
        protected bool SandboxMode { get; set; }

        protected OltEnableSandboxArgs()
        {
        }

        /// <summary>
        /// Enables Sandbox mode in Sendgrid message
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.SendGrid")]
        public T EnableSandbox()
        {
            this.SandboxMode = true;
            return (T)this;
        }
    }
}
