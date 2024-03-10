using Newtonsoft.Json;
using System.Collections.Generic;

namespace OLT.Email.SendGrid
{
    public class OltSendGridResponseJson
    {
        [JsonProperty("errors")]
        public List<OltSendGridResponseErrorJson> Errors { get; set; } = new List<OltSendGridResponseErrorJson>();
    }
}
