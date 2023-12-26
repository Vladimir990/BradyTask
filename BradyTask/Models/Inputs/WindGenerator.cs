using System.Xml.Serialization;

namespace BradyTask.Models.Input
{
    public class WindGenerator
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Generation")]
        public Generation Generation { get; set; }

        [XmlElement("Location")]
        public string Location { get; set; }
    }
}
