#nullable disable
using System;

namespace OLT.Email
{
    [Obsolete("OLT.Email is being deprecated in favor of jcamp.FluentEmail")]
    public class OltSmtpServer 
    {
        public virtual string Host { get; set; }
        public virtual int? Port { get; set; }
        public virtual bool DisableSsl { get; set; }
        public virtual OltSmtpCredentials Credentials { get; set; } = new OltSmtpCredentials();
    }
}