using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Outputs
{
    public class ActualHeatRates
    {
        [XmlElement("ActualHeatRate")]
        public List<ActualHeatRate> ActualHeatRateList { get; set; }
    }
}
