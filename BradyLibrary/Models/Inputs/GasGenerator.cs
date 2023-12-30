using BradyTask.BusinessLogic.Models.Inputs;
using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Input
{
    public class GasGenerator : BaseGenerator
    {
        [XmlElement("EmissionsRating")]
        public double EmissionsRating { get; set; }
    }
}
