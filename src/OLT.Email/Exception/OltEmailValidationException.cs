using System;
using System.Collections.Generic;

namespace OLT.Email
{

#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltEmailValidationException : OLT.Core.OltException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public const string DefaultMessage = "Email Validation Errors";

        protected  OltEmailValidationException(string message) : base(message)
        {

        }

        public OltEmailValidationException(List<string> errors) : this(DefaultMessage)
        {
            Errors = errors;
        }

        public OltEmailValidationException(List<string> errors, string message) : this(message)
        {
            Errors = errors;
        }


        public List<string> Errors { get;  }

        public OltEmailResult ToEmailResult()
        {
            return new OltEmailResult
            {
                Errors = Errors
            };
        }
    }
}
