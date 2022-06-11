using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class Geraet
    {
        /// <remarks/>
        [XmlElement("present")]
        public bool Present { get; set; }

        /// <remarks/>
        [XmlElement("txbusy")]
        public bool Txbusy { get; set; }

        /// <remarks/>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <remarks/>
        [XmlElement("battery")]
        public double Battery { get; set; }

        /// <remarks/>
        [XmlElement("batterylow")]
        public bool Batterylow { get; set; }

        /// <remarks/>
        [XmlElement("temperature")]
        public Temperatur Temperature { get; set; }

        /// <remarks/>
        [XmlElement("hkr")]
        public Heizkoerperregler Hkr { get; set; }

        /// <remarks/>
        [XmlAttribute("identifier")]
        public string Identifier { get; set; }

        /// <remarks/>
        [XmlAttribute("id")]
        public byte Id { get; set; }

        /// <remarks/>
        [XmlAttribute("functionbitmask")]
        public ushort Functionbitmask { get; set; }

        /// <remarks/>
        [XmlAttribute("fwversion")]
        public decimal Fwversion { get; set; }

        /// <remarks/>
        [XmlAttribute("manufacturer")]
        public string Manufacturer { get; set; }

        /// <remarks/>
        [XmlAttribute("productname")]
        public string Productname { get; set; }
    }
}