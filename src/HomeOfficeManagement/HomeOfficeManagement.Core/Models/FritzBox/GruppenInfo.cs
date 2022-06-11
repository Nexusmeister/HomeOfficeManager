using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class GruppenInfo
    {
        /// <remarks/>
        [XmlElement("masterdeviceid")]
        public byte Masterdeviceid { get; set; }

        /// <remarks/>
        [XmlElement("members")]
        public string Members { get; set; }
    }
}