using BradyTask.BusinessLogic.Models.Inputs;
using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Input
{
    public class CoalGenerator : BaseGenerator
    {
        [XmlElement("TotalHeatInput")]
        public double TotalHeatInput { get; set; }

        [XmlElement("ActualNetGeneration")]
        public double ActualNetGeneration { get; set; }

        [XmlElement("EmissionsRating")]
        public double EmissionsRating { get; set; }
    }
}
