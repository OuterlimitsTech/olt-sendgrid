using System.Collections.Generic;
using System.Linq;

namespace OLT.Email
{
    /// <summary>
    /// Whitelist of top-level domains and emails for sending emails in non-production environments
    /// </summary>
    public class OltEmailConfigurationWhitelist
    {
        /// <summary>
        /// Semicolon delimited string of top-level domains for sending emails in non-production environments
        /// </summary>
        /// <example>
        /// mydomain.com;mydomain.io
        /// </example>
        public virtual string Domain { get; set; }

        /// <summary>
        /// Parsed <seealso cref="Domain"/> 
        /// </summary>
        public virtual IEnumerable<string> DomainParsed => Domain?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)) ?? new List<string>();

        /// <summary>
        /// Semicolon delimited string of email addresses for sending emails in non-production environments
        /// </summary>
        /// <example>
        /// john.doe@fake-email.com;jane.smith@fake-email.com
        /// </example>
        public virtual string Email { get; set; }

        /// <summary>
        /// Parsed <seealso cref="Email"/> 
        /// </summary>
        public virtual IEnumerable<string> EmailParsed => Email?.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)) ?? new List<string>();
    }
}