using System.Xml.Serialization;

namespace BradyTask.Models.References
{
    [XmlRoot("ReferenceData")]
    public class ReferenceData
    {
        public Factors Factors { get; set; }
    }
}
