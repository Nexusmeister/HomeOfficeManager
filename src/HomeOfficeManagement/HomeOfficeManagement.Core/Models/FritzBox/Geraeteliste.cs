namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class Geraeteliste
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElement("device")]
        public Geraet[] Device { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("group")]
        public Gruppe[] Group { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute]
        public byte Version { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute]
        public decimal Fwversion { get; set; }
    }
}