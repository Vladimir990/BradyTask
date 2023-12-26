using BradyTask.Models.Helpers;
using BradyTask.Models.Input;
using BradyTask.Models.Outputs;
using BradyTask.Models.References;

namespace BradyTask
{
    public static class Calculations
    {
        public static GenerationOutput Calculate(GenerationReport generationReport, ReferenceData referenceData)
        {
            GenerationOutput output = new GenerationOutput();
            output.Totals = new Totals();
            output.Totals.Generators = new List<Generator>();
            output.MaxEmissionGenerators = new MaxEmissionGenerators();
            output.MaxEmissionGenerators.Days = new List<BradyTask.Models.Outputs.Day>();
            output.ActualHeatRates = new ActualHeatRates();
            output.ActualHeatRates.ActualHeatRateList = new List<ActualHeatRate>();
            List<DailyEmission> dailyEmission = new List<DailyEmission>();

            // Calculations for Total Generation Value, Daily Emissions, and Actual Heat Rate
            foreach (var windGenerator in generationReport.Wind.WindGenerators)
            {
                double totalGenerationValue = 0;
                foreach (var day in windGenerator.Generation.Days)
                {
                    if (windGenerator.Location == "Offshore")
                        totalGenerationValue += CalculateTotalGenerationValue(day.Energy, day.Price, referenceData.Factors.ValueFactor.Low);
                    else
                        totalGenerationValue += CalculateTotalGenerationValue(day.Energy, day.Price, referenceData.Factors.ValueFactor.High);
                }
                output.Totals.Generators.Add(new Generator { Name = windGenerator.Name, Total = totalGenerationValue });
            }

            foreach (var gasGenerator in generationReport.Gas.GasGenerators)
            {
                double totalGenerationValue = 0;
                foreach (var day in gasGenerator.Generation.Days)
                {
                    totalGenerationValue += CalculateTotalGenerationValue(day.Energy, day.Price, referenceData.Factors.ValueFactor.Medium);
                    dailyEmission.Add(CalculateDailyEmission(day.Energy, gasGenerator.Name, gasGenerator.EmissionsRating, referenceData.Factors.EmissionsFactor.Medium, day.Date));
                }
                output.Totals.Generators.Add(new Generator { Name = gasGenerator.Name, Total = totalGenerationValue });
            }

            foreach (var coalGenerator in generationReport.Coal.CoalGenerators)
            {
                double totalGenerationValue = 0;
                foreach (var day in coalGenerator.Generation.Days)
                {
                    totalGenerationValue += CalculateTotalGenerationValue(day.Energy, day.Price, referenceData.Factors.ValueFactor.Medium);
                    dailyEmission.Add(CalculateDailyEmission(day.Energy, coalGenerator.Name, coalGenerator.EmissionsRating, referenceData.Factors.EmissionsFactor.High, day.Date));
                }
                output.Totals.Generators.Add(new Generator { Name = coalGenerator.Name, Total = totalGenerationValue });
                output.ActualHeatRates.ActualHeatRateList.Add(new ActualHeatRate { Name = coalGenerator.Name, HeatRate = CalculateActualHeatRate(coalGenerator.TotalHeatInput, coalGenerator.ActualNetGeneration) });
            }

            var highest = dailyEmission
                .GroupBy(model => model.Date)
                .Select(group => group.OrderByDescending(model => model.Emission).FirstOrDefault())
                .ToList();

            foreach (var item in highest)
            {
                output.MaxEmissionGenerators.Days.Add(new BradyTask.Models.Outputs.Day { Name = item.Name, Date = item.Date, Emission = item.Emission });
            }

            return output;
        }

        private static double CalculateTotalGenerationValue(double energy, double price, double valueFactor)
        {
            return energy * price * valueFactor;
        }

        private static DailyEmission CalculateDailyEmission(double energy, string name, double emissionRating, double emissionFactor, DateTime date)
        {
            return new DailyEmission
            {
                Name = name,
                Date = date,
                Emission = energy * emissionRating * emissionFactor
            };
        }

        private static double CalculateActualHeatRate(double totalHeatInput, double actualNetGeneration)
        {
            // Avoid division by zero
            if (actualNetGeneration == 0)
            {
                return 0;
            }

            return totalHeatInput / actualNetGeneration;
        }
    }
}
