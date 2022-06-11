using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class Gruppe
    {
        /// <remarks/>
        [XmlElement("present")]
        public byte Present { get; set; }

        /// <remarks/>
        [XmlElement("txbusy")]
        public byte Txbusy { get; set; }

        /// <remarks/>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <remarks/>
        [XmlElement("hkr")]
        public Heizkoerperregler Hkr { get; set; }

        /// <remarks/>
        [XmlElement("groupinfo")]
        public GruppenInfo Groupinfo { get; set; }

        /// <remarks/>
        [XmlAttribute("synchronized")]
        public byte Synchronized { get; set; }

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