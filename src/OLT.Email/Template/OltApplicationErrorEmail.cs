using System;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public class OltApplicationErrorEmail : OltSmtpEmail, IOltApplicationErrorEmail
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string AppName { get; set; }
        public string Environment { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}