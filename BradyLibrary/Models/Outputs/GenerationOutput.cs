using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Outputs
{
    [XmlRoot("GenerationOutput")]
    public class GenerationOutput
    {
        public Totals Totals { get; set; } = new Totals();
        public MaxEmissionGenerators MaxEmissionGenerators { get; set; } = new MaxEmissionGenerators();
        public ActualHeatRates ActualHeatRates { get; set; } = new ActualHeatRates();
    }
}
