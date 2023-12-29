using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Outputs
{
    [XmlRoot("GenerationOutput")]
    public class GenerationOutput
    {
        public Totals Totals { get; set; }
        public MaxEmissionGenerators MaxEmissionGenerators { get; set; }
        public ActualHeatRates ActualHeatRates { get; set; }
    }
}
