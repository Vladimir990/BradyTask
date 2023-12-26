using System.Xml.Serialization;

namespace BradyTask.Models.Input
{
    public class Gas
    {
        [XmlElement("GasGenerator")]
        public List<GasGenerator> GasGenerators { get; set; }
    }
}
