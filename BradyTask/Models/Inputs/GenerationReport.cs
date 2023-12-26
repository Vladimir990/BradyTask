using System.Xml.Serialization;

namespace BradyTask.Models.Input
{
    public class GenerationReport
    {
        [XmlElement("Wind")]
        public Wind Wind { get; set; }
        [XmlElement("Gas")]
        public Gas Gas { get; set; }
        [XmlElement("Coal")]
        public Coal Coal { get; set; }
    }
}
