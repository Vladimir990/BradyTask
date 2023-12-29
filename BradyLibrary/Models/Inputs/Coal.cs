using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Input
{
    public class Coal
    {
        [XmlElement("CoalGenerator")]
        public List<CoalGenerator> CoalGenerators { get; set; }
    }
}
