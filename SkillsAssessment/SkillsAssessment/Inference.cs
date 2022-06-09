using System;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;

namespace SkillsAssessment
{
    public class Inference
    {
        public InferenceEngine Engine { get; set; }
        public Variable C_sharp { get; set; }
        public Variable Sql { get; set; }
        public PriorProbs Posteriors { get; protected set; }

        public void PerformInference()
        {
            var posteriorC_sharp = Engine.Infer<Bernoulli>(C_sharp);
            var posteriorSql = Engine.Infer<Bernoulli>(Sql);
            Posteriors = new PriorProbs
            {
                C_sharp = Math.Exp(posteriorC_sharp.GetLogProbTrue()),
                Sql = Math.Exp(posteriorSql.GetLogProbTrue())
            };

        }

    }

}
