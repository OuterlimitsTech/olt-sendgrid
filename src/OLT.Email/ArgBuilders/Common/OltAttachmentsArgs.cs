using System;
using System.Collections.Generic;

namespace OLT.Email
{
    public abstract class OltAttachmentsArgs<T> : OltRecipientsArgs<T> 
      where T : OltAttachmentsArgs<T>
    {
        protected List<OltEmailAttachment> Attachments { get; set; } = new List<OltEmailAttachment>();

        protected OltAttachmentsArgs()
        {
        }

        /// <summary>
        /// Add Attachment to Email
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public T WithAttachment(OltEmailAttachment value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            Attachments.Add(value);
            return (T)this;
        }

    
    }

}
