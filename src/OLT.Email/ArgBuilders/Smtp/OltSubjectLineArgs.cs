using OLT.Constants;
using System;
using System.Collections.Generic;

namespace OLT.Email
{
    public abstract class OltSubjectLineArgs<T> : OltSmtpPortArgs<T>
       where T : OltSubjectLineArgs<T>
    {
        protected  string SubjectLine { get; set; }

        protected OltSubjectLineArgs()
        {
        }

        /// <summary>
        /// Email Subject
        /// </summary>
        /// <returns></returns>
        [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
        public T WithSubject(string subject)
        {
            this.SubjectLine = subject ?? throw new ArgumentNullException(nameof(subject));
            return (T)this;
        }       

        public override List<string> ValidationErrors()
        {
            var errors = base.ValidationErrors();
            if (string.IsNullOrWhiteSpace(SubjectLine))
            {
                errors.Add(OltSmtpArgErrors.Subject);
            }
            return errors;
        }

    }

   
}
