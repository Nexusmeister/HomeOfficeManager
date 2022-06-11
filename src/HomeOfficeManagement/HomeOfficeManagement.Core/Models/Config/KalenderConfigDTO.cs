using System.Collections.Generic;

namespace HomeOfficeManagement.Core.Models.Config
{
    public class KalenderConfigDTO
    {
        public string KalenderId { get; set; }
        public IEnumerable<string> Terminnamen { get; set; }
    }
}