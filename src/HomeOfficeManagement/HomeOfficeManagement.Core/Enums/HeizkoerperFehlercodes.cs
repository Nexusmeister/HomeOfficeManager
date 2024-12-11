using System;
using System.Xml.Serialization;

namespace HomeOfficeManagement.Core.Enums
{
    [Serializable]
    public enum HeizkoerperFehlercodes
    {
        [XmlEnum("0")]
        KeinFehler = 0,
        [XmlEnum("1")]
        KeineAdaptierungMoeglich = 1,
        [XmlEnum("2")]
        VentilhubZuKurzOderBatterieZuSchwach = 2,
        [XmlEnum("3")]
        KeineVentilbewegungMoeglich = 3,
        [XmlEnum("4")]
        Installationsvorbereitung = 4,
        [XmlEnum("5")]
        Installationsmodus = 5,
        [XmlEnum("6")]
        AnpassungAnHeizungsventilhub = 6
    }
}