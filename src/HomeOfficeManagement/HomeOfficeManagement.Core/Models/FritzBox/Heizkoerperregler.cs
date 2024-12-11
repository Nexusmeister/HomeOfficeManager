using System.Xml.Serialization;
using HomeOfficeManagement.Core.Enums;

namespace HomeOfficeManagement.Core.Models.FritzBox
{
    public class Heizkoerperregler
    {
        /// <remarks/>
        public byte? IstTemperatur 
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Tist))
                {
                    byte.TryParse(Tist, out var tist);
                    return tist;
                }

                return null;
            }
        }

        [XmlElement("tist")]
        public string Tist { get; set; }

        /// <remarks/>
        [XmlElement("tsoll")]
        public byte Tsoll { get; set; }

        /// <remarks/>
        [XmlElement("absenk")]
        public byte Absenk { get; set; }

        /// <remarks/>
        [XmlElement("komfort")]
        public byte Komfort { get; set; }

        /// <remarks/>
        [XmlElement("lock")]
        public byte Lock { get; set; }

        /// <remarks/>
        [XmlElement("devicelock")]
        public byte Devicelock { get; set; }

        /// <remarks/>
        [XmlElement("errorcode")]
        public HeizkoerperFehlercodes Errorcode { get; set; }

        /// <remarks/>
        [XmlElement("windowopenactiv")]
        public byte Windowopenactiv { get; set; }

        /// <remarks/>
        [XmlElement("windowopenactiveendtime")]
        public byte Windowopenactiveendtime { get; set; }

        /// <remarks/>
        [XmlElement("boostactive")]
        public byte Boostactive { get; set; }

        /// <remarks/>
        [XmlElement("boostactiveendtime")]
        public byte Boostactiveendtime { get; set; }

        /// <remarks/>
        [XmlElement("batterylow")]
        public bool Batterylow { get; set; }

        /// <remarks/>
        [XmlElement("battery")]
        public byte Battery { get; set; }

        /// <remarks/>
        [XmlElement("nextchange")]
        public NaechsteVeraenderung Nextchange { get; set; }

        /// <remarks/>
        [XmlElement("summeractive")]
        public byte Summeractive { get; set; }

        /// <remarks/>
        [XmlElement("holidayactive")]
        public byte Holidayactive { get; set; }
    }
}