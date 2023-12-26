using System.Xml.Serialization;

namespace BradyTask.Models.Outputs
{
    public class MaxEmissionGenerators
    {
        [XmlElement("Day")]
        public List<Day> Days { get; set; }
    }
}
