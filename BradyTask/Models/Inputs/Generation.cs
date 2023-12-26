using System.Xml.Serialization;

namespace BradyTask.Models.Input
{
    public class Generation
    {
        [XmlElement("Day")]
        public List<Day> Days { get; set; }
    }
}
