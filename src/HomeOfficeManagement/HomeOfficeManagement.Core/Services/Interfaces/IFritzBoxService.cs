using System.Collections.Generic;
using System.Threading.Tasks;
using HomeOfficeManagement.Core.Models;
using HomeOfficeManagement.Core.Models.FritzBox;

namespace HomeOfficeManagement.Core.Services.Interfaces
{
    public interface IFritzBoxService
    {
        Task<string> GetSessionId(string benutzername, string kennwort);
        Task<Geraeteliste> GetAlleGeraete(string sessionId);
        Task<Geraet> GetGeraet(string sessionId, string identifier);
        Task<Gruppe> GetGruppeByGeraet(Geraet device, string sessionId);

        Task<bool> SetTemplateByHomeOffice(Gruppe deviceGruppe, IEnumerable<HomeOfficeDTO> wocheneinstellung,
            string sessionId);

        Task<Vorlagenliste> GetVorlagen(string sessionId);
    }
}