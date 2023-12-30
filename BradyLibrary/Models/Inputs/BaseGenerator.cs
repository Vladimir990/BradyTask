using BradyTask.BusinessLogic.Models.Input;
using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Inputs
{
    public class BaseGenerator
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Generation")]
        public Generation Generation { get; set; }
    }
}
