using System.Threading.Tasks;
using HomeOfficeManagement.Core.Config.Interfaces;
using HomeOfficeManagement.Core.Models.Config;
using HomeOfficeManagement.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeOfficeManagement
{
    public class ManagerEngine
    {
        private readonly IKalenderService _service;
        private readonly IFritzBoxService _fritzBoxService;
        private readonly ILogger<ManagerEngine> _logger;
        private readonly HomeOfficeManagerConfigDTO _config;

        public ManagerEngine(ILogger<ManagerEngine> logger,
            IKalenderService kalenderService, 
            IFritzBoxService fritzBoxService,
            IConfigLoader<HomeOfficeManagerConfigDTO> configLoader)
        {
            _logger = logger;
            _service = kalenderService;
            _fritzBoxService = fritzBoxService;
            _config = configLoader.GetConfig();
        }

        public async Task StarteEngine()
        {
            _logger.LogInformation("Beginne mit Abarbeitung durch Start der Engine");
            _logger.LogInformation("Aktuelle KalenderService-Instanz: {serviceType}", _service.GetType());
            var homeOffice = await _service.GetKalenderUebersichtFuerNaechsteWoche();

            var benutzername = _config.FritzBox.FritzBoxUser.Benutzername;
            var kennwort = _config.FritzBox.FritzBoxUser.Passwort;
            var sessionId = await _fritzBoxService.GetSessionId(benutzername, kennwort);

            var thermostat = await _fritzBoxService.GetGeraet(sessionId, "09995 0801942");
            var gruppe = await _fritzBoxService.GetGruppeByGeraet(thermostat, sessionId);
            var vorlagen = await _fritzBoxService.GetVorlagen(sessionId);

            _logger.LogInformation("Verarbeitung wurde erledigt.");
        }

    }
}