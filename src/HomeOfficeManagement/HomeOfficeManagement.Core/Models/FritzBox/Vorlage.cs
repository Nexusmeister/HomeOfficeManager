using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class Vorlage
    {
        /// <remarks/>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <remarks/>
        [XmlElement("devices", IsNullable = false)]
        public VorlageGruppeZuordnung[] Devices { get; set; }

        /// <remarks/>
        [XmlElement("applymask")]
        public TemplateEinstellung Einstellungen { get; set; }

        /// <remarks/>
        [XmlAttribute("identifier")]
        public string Identifier { get; set; }

        /// <remarks/>
        [XmlAttribute("id")]
        public ushort Id { get; set; }

        /// <remarks/>
        [XmlAttribute("functionbitmask")]
        public ushort Functionbitmask { get; set; }

        /// <remarks/>
        [XmlAttribute("applymask")]
        public byte Applymask { get; set; }
    }
}