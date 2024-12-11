using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HomeOfficeManagement.Core.Models;

namespace HomeOfficeManagement.Core
{
    public class CalendarServiceFacade
    {
        public static async Task Test()
        {
            var i = new BaseClientService.Initializer();


            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { CalendarService.Scope.CalendarEventsReadonly },
                    "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));
            }

            var d = new CalendarService(new BaseClientService.Initializer
            {
                ApplicationName = "AppName",
                HttpClientInitializer = credential
            });

            var y = d.Events.List("xxx");
            y.TimeMin = DateTime.Today;
            y.TimeMax = DateTime.Today.AddDays(7);
            y.MaxResults = 50;
            y.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            y.ShowDeleted = true;

            var res = await y.ExecuteAsync();

            var listeWochentage = new List<HomeOfficeDTO>();

            var itemsInZukunft = res.Items.Where(x =>
                x.Start.DateTime.GetValueOrDefault(DateTime.Today.AddDays(-7)) >= DateTime.Now);
            
            var gruppierteResults =
                itemsInZukunft
                    .GroupBy(x => x.Start.DateTime.GetValueOrDefault(DateTime.Now))
                    .Select(grp => grp);

            foreach (var terminJeTag in gruppierteResults)
            {
                var wochentag = new HomeOfficeDTO
                {
                    Datum = terminJeTag.Key
                };

                var arbeitsliste = terminJeTag.ToList();
                if (arbeitsliste.Any(x => x.Summary.Equals("Home Office") || x.Summary.Equals("HO")))
                {
                    wochentag.IstHomeOffice = true;
                }
                else
                {
                    wochentag.IstHomeOffice = false;
                }

                listeWochentage.Add(wochentag);
            }
        }
    }
}