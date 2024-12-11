using System.IO;
using System.Text;
using System.Threading.Tasks;
using HomeOfficeManagement.Core.Config.Interfaces;
using HomeOfficeManagement.Core.Models;
using HomeOfficeManagement.Core.Models.Config;
using Newtonsoft.Json;

namespace HomeOfficeManagement.Core.Config.Implementations
{
    public class HomeOfficeManagerConfigLoader : IConfigLoader<HomeOfficeManagerConfigDTO>
    {
        public HomeOfficeManagerConfigDTO GetConfig()
        {
            using var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read);
            string result;

            using (var streamReader = new StreamReader(stream, Encoding.Default))
            {
                result = streamReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<HomeOfficeManagerConfigDTO>(result);
        }
    }
}