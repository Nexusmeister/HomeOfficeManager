using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace HomeOfficeManagement.Core.Models.Config
{
    public class HomeOfficeManagerConfigDTO
    {
        [JsonProperty("installed")]
        public ClientSecrets ClientSecrets { get; set; }
        public FritzBoxDTO FritzBox { get; set; }
        public KalenderConfigDTO Kalender { get; set; }
    }
}