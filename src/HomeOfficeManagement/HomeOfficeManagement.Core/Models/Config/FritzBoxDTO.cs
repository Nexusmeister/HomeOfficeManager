using HomeOfficeManagement.Core.Models.FritzBox;

namespace HomeOfficeManagement.Core.Models.Config
{
    public class FritzBoxDTO
    {
        public FritzBoxUser FritzBoxUser { get; set; }
        public string LoginPage { get; set; }
        public string WebserviceBaseUri { get; set; }
    }
}