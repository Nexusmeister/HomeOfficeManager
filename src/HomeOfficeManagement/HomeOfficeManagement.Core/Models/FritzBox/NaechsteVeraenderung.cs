using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class NaechsteVeraenderung
    {
        /// <remarks/>
        [XmlElement("endperiod")]
        public uint Endperiod { get; set; }

        /// <remarks/>
        [XmlElement("tchange")]
        public byte Tchange { get; set; }
    }
}