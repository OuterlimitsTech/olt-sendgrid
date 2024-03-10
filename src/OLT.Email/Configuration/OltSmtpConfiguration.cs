using System;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public class OltSmtpConfiguration : OltEmailConfiguration
    {
        public OltSmtpServer Smtp {  get; set; } = new OltSmtpServer();
    }
}