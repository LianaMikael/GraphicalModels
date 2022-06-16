using System;
using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Models;

namespace GameOutcomes
{
    public class SkillModel
    {
        public Gaussian JSkillPredicted { get; set; }
        public Gaussian FSkillPredicted { get; set; }

        public SkillModel(bool observedValue)
        {
            var priors = new Priors { JSkillMean = 120, JSkillVariance = 1600, FSkillMean = 100, FSkillVariance = 25 };
            
            var JSkill = Variable.GaussianFromMeanAndVariance(priors.JSkillMean, priors.JSkillVariance).Named("JSkill");
            var FSkill = Variable.GaussianFromMeanAndVariance(priors.FSkillMean, priors.FSkillVariance).Named("FSkill");

            var JPerf = Variable.GaussianFromMeanAndVariance(JSkill, 25); 
            var FPerf = Variable.GaussianFromMeanAndVariance(FSkill, 25);

            var Jwins = JPerf > FPerf;

            Jwins.ObservedValue = observedValue;

            var engine = new InferenceEngine(new ExpectationPropagation());

            JSkillPredicted = engine.Infer<Gaussian>(JSkill);
            FSkillPredicted = engine.Infer<Gaussian>(FSkill);
        }
    }

    public class Priors
    {
        public double JSkillMean { get; set; }
        public double JSkillVariance { get; set; }
        public double FSkillMean { get; set; }
        public double FSkillVariance { get; set; }
    }

    
}
