using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Outputs
{
    public class MaxEmissionGenerators
    {
        [XmlElement("Day")]
        public List<Day> Days { get; set; } = new List<Day>();
    }
}
