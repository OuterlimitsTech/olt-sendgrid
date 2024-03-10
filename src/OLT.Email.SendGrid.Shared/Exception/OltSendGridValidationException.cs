using System;
using System.Collections.Generic;

namespace OLT.Email.SendGrid
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltSendGridValidationException : OltEmailValidationException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public new const string DefaultMessage = "SendGrid Validation Errors";

        public OltSendGridValidationException(List<string> errors) : base(errors, DefaultMessage)
        {
            
        }


    }
}
