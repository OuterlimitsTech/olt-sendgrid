#nullable disable
using Newtonsoft.Json;
using System;
using System.Text;

namespace OLT.Email.SendGrid
{
    public class OltSendGridResponseErrorJson
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("help")]
        public string Help { get; set; }
    }
}
