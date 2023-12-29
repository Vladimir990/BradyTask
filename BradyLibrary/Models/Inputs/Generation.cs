using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Input
{
    public class Generation
    {
        [XmlElement("Day")]
        public List<Day> Days { get; set; }
    }
}
