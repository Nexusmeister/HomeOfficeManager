using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using HomeOfficeManagement.Core.Config.Interfaces;
using HomeOfficeManagement.Core.Models;
using HomeOfficeManagement.Core.Models.Config;
using HomeOfficeManagement.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeOfficeManagement.Core.Services.Implementations
{
    public class GoogleKalenderService : IKalenderService
    {
        private readonly ILogger<GoogleKalenderService> _logger;
        private readonly HomeOfficeManagerConfigDTO _config;
        private readonly CalendarService _calendar;
        private readonly IFritzBoxService _fritzBoxService;

        private int _retryCounter;

        public GoogleKalenderService(ILogger<GoogleKalenderService> logger,
            IConfigLoader<HomeOfficeManagerConfigDTO> configLoader,
            CalendarService calendarService,
            IFritzBoxService fritzBoxService)
        {
            _logger = logger;
            _config = configLoader.GetConfig();
            _calendar = calendarService;
            _fritzBoxService = fritzBoxService;
        }

        public async Task<IEnumerable<HomeOfficeDTO>> GetKalenderUebersichtFuerNaechsteWoche()
        {
            try
            {
                _logger.LogTrace("Starte Abarbeitung von {methode}", nameof(GetKalenderUebersichtFuerNaechsteWoche));
                _logger.LogDebug("Beginne mit Aufbau der Abfrage");
                _logger.LogInformation("Aktueller Kalender im Zugriff: {kalender}", _config.Kalender.KalenderId);
                var kalenderRequest = _calendar.Events.List(_config.Kalender.KalenderId);
                kalenderRequest.TimeMin = GetNaechstenMontag(); // Montag
                kalenderRequest.TimeMax = DateTime.Today.AddDays(6); // Nächster Freitag
                kalenderRequest.MaxResults = 50;
                kalenderRequest.ShowDeleted = true;

                _logger.LogTrace("Starte Abfrage gegen Google API");
                var res = await kalenderRequest.ExecuteAsync();
                _logger.LogTrace("Abfrage gegen Google API beendet");
                _logger.LogInformation("{count} Datensätze wurden geladen", res.Items.Count);

                var listeHomeOffice = new List<HomeOfficeDTO>();
                var tagBuffer = GetNaechstenMontag();
                for (var i = 0; i < 5; i++)
                {
                    listeHomeOffice.Add(new HomeOfficeDTO
                    {
                        Datum = tagBuffer.Date
                    });

                    tagBuffer = tagBuffer.AddDays(1);
                }

                var termine = (from termin in res.Items
                    where _config.Kalender.Terminnamen.Contains(termin.Summary)
                    select termin).ToList();
                _logger.LogInformation("Es wurde(n) {count} Home Office Tag(e) gefunden", termine.Count);

                _logger.LogTrace("Starte mit Verarbeitung der gefilterten Termine");
                foreach (var termin in termine)
                {
                    _logger.LogInformation("Termin {terminId} wird verarbeitet", termin.Id);
                    DateTime start;
                    DateTime ende;

                    if (termin.Start.DateTime.HasValue)
                    {
                        start = termin.Start.DateTime.GetValueOrDefault();
                    }
                    else
                    {
                        DateTime.TryParse(termin.Start.Date, out start);
                    }

                    if (termin.End.DateTime.HasValue)
                    {
                        ende = termin.Start.DateTime.GetValueOrDefault();
                    }
                    else
                    {
                        DateTime.TryParse(termin.End.Date, out ende);
                    }

                    _logger.LogDebug("Startdatum: {start} / Endzeitpunkt: {ende}", start, ende);

                    // Falls ein Termin über mehrere Tage gelegt ist
                    if (ende > start)
                    {
                        _logger.LogTrace("Wir haben einen Termin, der über mehrere Tage geht");
                        var anzahlTage = (ende.Date - start.Date).TotalDays;
                        _logger.LogDebug("Termin dauert {tage} Tage", anzahlTage);

                        var terminBuffer = start.Date;
                        for (var i = 0; i < anzahlTage; i++)
                        {
                            var ho = listeHomeOffice.FirstOrDefault(x => x.Datum.Equals(terminBuffer));
                            if (ho is null)
                            {
                                _logger.LogError("{datum} Eintrag ist nicht vorhanden. Fehler in der Wochenerstellung!", terminBuffer.Date);
                            }
                            else
                            {
                                ho.IstHomeOffice = true;
                            }

                            terminBuffer = terminBuffer.AddDays(1);
                        }
                    }
                    else
                    {
                        var ho = listeHomeOffice.FirstOrDefault(x => x.Datum.Equals(start));
                        if (ho is null)
                        {
                            _logger.LogError("{datum} Eintrag ist nicht vorhanden. Fehler in der Wochenerstellung!", start.Date);
                        }
                        else
                        {
                            ho.IstHomeOffice = true;
                        }
                    }
                }

                _logger.LogInformation("Es wurden {count} Home Office Tage ermittelt", listeHomeOffice.Count(x => x.IstHomeOffice));

                return listeHomeOffice;
            }
            catch (Exception e1)
            {
                if (_retryCounter < 3)
                {
                    _retryCounter++;
                    _logger.LogError(e1, "Fehler in der Abarbeitung. Wir probieren Versuch #{versuch}", _retryCounter);
                    return await GetKalenderUebersichtFuerNaechsteWoche();
                }

                _logger.LogError(e1, "Fehler in der Abarbeitung. Es wurde Versuch #{versuch} erreicht. Breche Abarbeitung ab.", _retryCounter);
                return null;
            }
        }

        private static DateTime GetNaechstenMontag()
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            var tageBisMontag = ((int)DayOfWeek.Monday - (int)DateTime.Today.DayOfWeek + 7) % 7;
            return DateTime.Today.Date.AddDays(tageBisMontag);
        }
    }
}