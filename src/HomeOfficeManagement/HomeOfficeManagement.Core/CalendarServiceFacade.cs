using Google.Apis.Services;

namespace HomeOfficeManagement.Core
{
    public class CalendarServiceFacade
    {
        public static void Test()
        {
            var i = new BaseClientService.Initializer();
            i.ApiKey = "xxxx";

            var d = new Google.Apis.Calendar.v3.CalendarService();
            
            //d.ApiKey = "AIzaSyD50U-7YnP2YlwPhqv3QK30SusR1v7rk_c";
            var x = d.Calendars.Get("xxxx");

            var y = d.Events.List("xxxx");
        }
    }
}