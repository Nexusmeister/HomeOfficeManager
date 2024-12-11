using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class TemplateEinstellung
    {
        /// <remarks/>
        [XmlElement("hkr_time_table")]
        public object HkrTimeTable { get; set; }

        /// <remarks/>
        [XmlElement("hkr_holidays")]
        public object HkrHolidays { get; set; }
    }
}