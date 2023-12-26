using System.Xml.Serialization;

namespace BradyTask.Models.Input
{
    public class Day
    {
        [XmlElement("Date")]
        public DateTime Date { get; set; }

        [XmlElement("Energy")]
        public double Energy { get; set; }

        [XmlElement("Price")]
        public double Price { get; set; }
    }
}
