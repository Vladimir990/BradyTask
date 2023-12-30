using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Outputs
{
    public class Totals
    {
        [XmlElement("Generator")]
        public List<Generator> Generators { get; set; } = new List<Generator>();
    }
}
