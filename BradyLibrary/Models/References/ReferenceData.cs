using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.References
{
    [XmlRoot("ReferenceData")]
    public class ReferenceData
    {
        public Factors Factors { get; set; }
    }
}
