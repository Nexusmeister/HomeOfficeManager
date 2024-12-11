using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class Vorlagenliste
    {
        /// <remarks/>
        [XmlElement("template")]
        public Vorlage[] Templates { get; set; }

        /// <remarks/>
        [XmlAttribute("version")]
        public byte Version { get; set; }
    }
}