using System.Collections.Generic;
using System.Threading.Tasks;
using HomeOfficeManagement.Core.Models;

namespace HomeOfficeManagement.Core.Services.Interfaces
{
    public interface IKalenderService
    {
        Task<IEnumerable<HomeOfficeDTO>> GetKalenderUebersichtFuerNaechsteWoche();
        //Task<UserCredential> GetAuthentication();
    }
}