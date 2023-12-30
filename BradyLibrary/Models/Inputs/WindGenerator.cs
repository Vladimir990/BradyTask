using BradyTask.BusinessLogic.Models.Inputs;
using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Input
{
    public class WindGenerator : BaseGenerator
    {
        [XmlElement("Location")]
        public string Location { get; set; }
    }
}
