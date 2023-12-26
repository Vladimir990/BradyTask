using System.Xml.Serialization;

namespace BradyTask.Models.Outputs
{
    public class Totals
    {
        [XmlElement("Generator")]
        public List<Generator> Generators { get; set; }
    }
}
