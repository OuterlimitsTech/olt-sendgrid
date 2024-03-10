using System;

namespace OLT.Email
{
    public abstract class OltEmailArgsProduction<T> : OltEmailBuilderArgs
      where T : OltEmailArgsProduction<T>
    {
        private bool _enabled = false;

        protected  override bool Enabled => _enabled;        

        protected OltEmailArgsProduction()
        {
        }

        /// <summary>
        /// Sends emails for all requests and regardless whitelist values
        /// </summary>
        /// <param name="value"><see cref="bool"/></param>
        /// <returns><typeparamref name="T"/></returns>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.Extensions")]
        public T EnableProductionEnvironment(bool value)
        {
            this._enabled = value;
            return (T)this;
        }
    } 


}
