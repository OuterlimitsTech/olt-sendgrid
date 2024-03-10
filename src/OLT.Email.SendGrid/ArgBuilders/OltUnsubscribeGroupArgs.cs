using System;

namespace OLT.Email.SendGrid
{
    public abstract class OltUnsubscribeGroupArgs<T> : OltDisableClickTrackingArgs<T>
        where T : OltUnsubscribeGroupArgs<T>
    {
        protected int? UnsubscribeGroupId { get; set; }

        protected OltUnsubscribeGroupArgs()
        {
        }

        /// <summary>
        /// Send Grid Template
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [Obsolete("OLT.Email.SendGrid is being deprecated in favor of OLT.FluentEmail.SendGrid")]
        public T WithUnsubscribeGroupId(int unsubscribeGroupId)
        {
            if (unsubscribeGroupId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(unsubscribeGroupId), "must be greater than zero");
            }
            this.UnsubscribeGroupId = unsubscribeGroupId;
            return (T)this;
        }

     
    }
}
