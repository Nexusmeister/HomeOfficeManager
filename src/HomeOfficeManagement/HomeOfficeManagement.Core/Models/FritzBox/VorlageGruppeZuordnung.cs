using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class VorlageGruppeZuordnung
    {
        /// <remarks/>
        [XmlElement("identifier")]
        public string Identifier { get; set; }
    }
}