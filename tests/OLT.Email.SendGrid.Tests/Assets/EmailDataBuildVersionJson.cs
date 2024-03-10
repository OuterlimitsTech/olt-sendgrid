using System;

namespace OLT.Email.SendGrid.Tests.Assets
{

    public class EmailDataBuildVersionJson
    {
        public string Version { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
        public string Info => $"{Version} at {Date:F}";
    }
}