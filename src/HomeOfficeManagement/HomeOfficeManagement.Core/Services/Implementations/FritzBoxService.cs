using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using HomeOfficeManagement.Core.Config.Interfaces;
using HomeOfficeManagement.Core.Models;
using HomeOfficeManagement.Core.Models.Config;
using HomeOfficeManagement.Core.Models.FritzBox;
using HomeOfficeManagement.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;
namespace HomeOfficeManagement.Core.Services.Implementations
{
    public class FritzBoxService : IFritzBoxService
    {
        private readonly ILogger<FritzBoxService> _logger;
        private readonly HomeOfficeManagerConfigDTO _config;

        public FritzBoxService(ILogger<FritzBoxService> logger,
            IConfigLoader<HomeOfficeManagerConfigDTO> configLoader)
        {
            _logger = logger;
            _config = configLoader.GetConfig();
        }

        public async Task<string> GetSessionId(string benutzername, string kennwort)
        {
            _logger.LogTrace("Ermittel SessionID für FritzBox in {methode}", nameof(GetSessionId));
            var seitenStream = await SeiteEinlesen(_config.FritzBox.LoginPage);
            var doc = await XDocument.LoadAsync(seitenStream, LoadOptions.SetBaseUri, CancellationToken.None);
            var sid = GetValue(doc, "SID");
            if (!sid.Equals("0000000000000000"))
            {
                return sid;
            }

            _logger.LogTrace("Challenge nötig...");
            var challenge = GetValue(doc, "Challenge");
            var uri = _config.FritzBox.LoginPage + $"?username={benutzername}&response={GetResponse(challenge, kennwort)}";
            doc = XDocument.Load(uri);
            sid = GetValue(doc, "SID");
            _logger.LogDebug("SessionID {sessionId} wurde für aktuellen Lauf ermittelt", sid);

            return sid;
        }

        public async Task<Geraeteliste> GetAlleGeraete(string sessionId)
        {
            _logger.LogTrace("{methode} beginnt Suche von allen Geräten", nameof(GetGeraet));
            var http = new HttpClient();
            http.BaseAddress = new Uri(_config.FritzBox.WebserviceBaseUri);
            var res = await http.GetStreamAsync(new Uri(http.BaseAddress,
                $"?switchcmd=getdevicelistinfos&sid={sessionId}"));

            var xRoot = new XmlRootAttribute
            {
                ElementName = "devicelist",
                IsNullable = true
            };

            var x = new XmlSerializer(typeof(Geraeteliste), xRoot);
            return (Geraeteliste)x.Deserialize(res);
        }

        public async Task<Geraet> GetGeraet(string sessionId, string identifier)
        {
            _logger.LogTrace("{methode} beginnt Suche von {identifier}", nameof(GetGeraet), identifier);
            var http = new HttpClient();
            http.BaseAddress = new Uri(_config.FritzBox.WebserviceBaseUri);
            var res = await http.GetStreamAsync(new Uri(http.BaseAddress,
                $"?switchcmd=getdeviceinfos&sid={sessionId}&ain={identifier}"));

            var xRoot = new XmlRootAttribute
            {
                ElementName = "device",
                IsNullable = true
            };

            var serializer = new XmlSerializer(typeof(Geraet), xRoot);
            var result = (Geraet)serializer.Deserialize(res);
            if (result is null)
            {
                _logger.LogError("Gerät mit Identifier {identifier} wurde nicht gefunden!", identifier);
                return null;
            }

            _logger.LogDebug("Gerät {devicename}({deviceId}) wurde gefunden", result.Name, result.Id);
            return result;
        }

        public async Task<Gruppe> GetGruppeByGeraet(Geraet device, string sessionId)
        {
            _logger.LogTrace("{methode} ermittelt zugehörige Gruppe", nameof(GetGruppeByGeraet));
            _logger.LogDebug("Gruppe von Gerät {deviceId} wird ermittelt", device.Identifier);
            var geraete = await GetAlleGeraete(sessionId);
            var gruppe = geraete
                .Group
                .Where(x => x
                    .Groupinfo
                    .Members
                    .Contains(device
                        .Id
                        .ToString()))
                .FirstOrDefault(x => x
                    .Name
                    .Contains("Robin"));

            if (gruppe is null)
            {
                _logger.LogError("Gruppe von Thermostat {thermostat} konnte nicht ermittelt werden. Ist Thermostat aktuell in einer Gruppe?", device.Name);
            }

            return gruppe;
        }

        public async Task<bool> SetTemplateByHomeOffice(Gruppe deviceGruppe, IEnumerable<HomeOfficeDTO> wocheneinstellung, string sessionId)
        {
            var vorlagen = await GetVorlagen(sessionId);


            var http = new HttpClient();
            http.BaseAddress = new Uri(_config.FritzBox.WebserviceBaseUri);
            var res = await http.GetStreamAsync(new Uri(http.BaseAddress,
                $"?switchcmd=gettemplatelistinfos&sid={sessionId}"));

            var xRoot = new XmlRootAttribute
            {
                ElementName = "device",
                IsNullable = true
            };

            var serializer = new XmlSerializer(typeof(Geraet), xRoot);
            var result = (Geraet)serializer.Deserialize(res);
            return true;
        }

        public async Task<Vorlagenliste> GetVorlagen(string sessionId)
        {
            var http = new HttpClient();
            http.BaseAddress = new Uri(_config.FritzBox.WebserviceBaseUri);
            var res = await http.GetStreamAsync(new Uri(http.BaseAddress,
                $"?switchcmd=gettemplatelistinfos&sid={sessionId}"));

            var xRoot = new XmlRootAttribute
            {
                ElementName = "templatelist",
                IsNullable = true
            };

            var serializer = new XmlSerializer(typeof(Vorlagenliste), xRoot);
            var result = (Vorlagenliste)serializer.Deserialize(res);
            return result;
        }

        private static string GetResponse(string challenge, string kennwort)
        {
            return challenge + "-" + GetMd5Hash(challenge + "-" + kennwort);
        }

        private static string GetMd5Hash(string input)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));
            var sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        private static string GetValue(XContainer doc, string name)
        {
            if (doc.FirstNode is XElement info)
            {
                return info.Element(name)?.Value;
            }

            return string.Empty;
        }

        private static async Task<Stream> SeiteEinlesen(string url)
        {
            var uri = new Uri(url);
            var request = WebRequest.CreateHttp(uri);
            if (await request.GetResponseAsync() is not HttpWebResponse response)
            {
                return null;
            }

            return response.GetResponseStream();
        }
    }
}