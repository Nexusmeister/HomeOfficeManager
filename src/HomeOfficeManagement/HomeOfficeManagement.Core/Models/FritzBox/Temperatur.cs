using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class Temperatur
    {
        /// <remarks/>
        [XmlElement("celsius")]
        public double Celsius { get; set; }

        [XmlElement("offset")]
        public double Offset { get; set; }
    }
}