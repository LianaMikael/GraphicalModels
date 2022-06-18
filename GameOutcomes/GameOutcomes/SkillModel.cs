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

    public class ThreePlayerModel
    {
        public Gaussian Player1SkillPredicted { get; set; }
        public Gaussian Player2SkillPredicted { get; set; }
        public Gaussian Player3SkillPredicted { get; set; }

        public ThreePlayerModel(bool player1Wins, bool player2Wins)
        {
            var priors = new Priors { JSkillMean = 120, JSkillVariance = 1600, FSkillMean = 100, FSkillVariance = 25, SteveMean = 140, SteveVariance = 1600 };

            var Player1Skill = Variable.GaussianFromMeanAndVariance(priors.JSkillMean, priors.JSkillVariance).Named("Player1Skill");
            var Player2Skill = Variable.GaussianFromMeanAndVariance(priors.FSkillMean, priors.FSkillVariance).Named("Player2Skill");
            var Player3Skill = Variable.GaussianFromMeanAndVariance(priors.SteveMean, priors.SteveVariance).Named("Player3Skill");

            var Player1Perf = Variable.GaussianFromMeanAndVariance(Player1Skill, 25);
            var Player2Perf = Variable.GaussianFromMeanAndVariance(Player2Skill, 25);
            var Player3Perf = Variable.GaussianFromMeanAndVariance(Player3Skill, 25);

            var Player1Wins = Player1Perf > Player2Perf;
            var Player2Wins = Player2Perf > Player3Perf;

            Player1Wins.ObservedValue = player1Wins;
            Player2Wins.ObservedValue = player2Wins;

            var engine = new InferenceEngine(new ExpectationPropagation());
            Player1SkillPredicted = engine.Infer<Gaussian>(Player1Skill);
            Player2SkillPredicted = engine.Infer<Gaussian>(Player2Skill);
            Player3SkillPredicted = engine.Infer<Gaussian>(Player3Skill);
        }
    }
    
    public class Priors
    {
        public double JSkillMean { get; set; }
        public double JSkillVariance { get; set; }
        public double FSkillMean { get; set; }
        public double FSkillVariance { get; set; }
        public double SteveMean { get; set; }
        public double SteveVariance { get; set; }
    }

    
}
