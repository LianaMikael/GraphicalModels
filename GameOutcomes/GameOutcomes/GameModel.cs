using System;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;

namespace GameOutcomes
{
    public class GameModel
    {
        public Bernoulli gameOutcome { get; set; }

        public GameModel()
        {
            var parameters = new Parameters { JMean = 15, JVariance = 25, FMean = 12.5, FVariance = 25};
            
            var JPerf = Variable.GaussianFromMeanAndVariance(parameters.JMean, parameters.JVariance).Named("JillsPerformance");
            var FPerf = Variable.GaussianFromMeanAndVariance(parameters.FMean, parameters.FVariance).Named("FredsPerformance");

            var Jwins = JPerf > FPerf;

            var engine = new InferenceEngine();
            gameOutcome = engine.Infer<Bernoulli>(Jwins);
        }
    }

    public class Parameters
    {
        public double JMean { get; set; }
        public double JVariance { get; set; }
        public double FMean { get; set; }
        public double FVariance { get; set; }
    }
}
