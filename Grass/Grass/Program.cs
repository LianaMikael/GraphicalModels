using System;
using Microsoft.ML.Probabilistic.Models;

namespace Grass
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Grass/Rain/Sprinkler Problem");

            // create the graphical model 
            var priors = new PriorProbs { Cloudy = 0.5 };
            var conditionalsSprinkler = new ConditionalVariablesSprinkler { SprinklerGivenCloudy = 0.1, SprinklerGivenNotCloudy = 0.5 };
            var conditionalsRain = new ConditionalVariablesRain { RainGivenCloudy = 0.8, RainGivenNotCloudy = 0.2 };
            var conditionalsWetGrass = new ConditionalVariablesGrass
            {
                WetGrassGivenSprinklerAndRain = 0.99,
                WetGrassGivenSprinklerNoRain = 0.9,
                WetGrassGivenNoSprinklerAndRain = 0.9,
                WetGrassGivenNoSprinklerNoRain = 0
            };

            var cloudy = Variable.Bernoulli(priors.Cloudy).Named("cloudy=True");
            var sprinkler = Variable.New<bool>().Named("sprinkler=True");
            var rain = Variable.New<bool>().Named("rain=True");
            var wetGrass = Variable.New<bool>().Named("wetGrass=True");

            // set conditionals
            using (Variable.If(cloudy)) sprinkler.SetTo(Variable.Bernoulli(conditionalsSprinkler.SprinklerGivenCloudy));
            using (Variable.IfNot(cloudy)) sprinkler.SetTo(Variable.Bernoulli(conditionalsSprinkler.SprinklerGivenNotCloudy));

            using (Variable.If(cloudy)) rain.SetTo(Variable.Bernoulli(conditionalsRain.RainGivenCloudy));
            using (Variable.IfNot(cloudy)) rain.SetTo(Variable.Bernoulli(conditionalsRain.RainGivenNotCloudy));

            using (Variable.If(sprinkler))
            {
                using (Variable.If(rain)) wetGrass.SetTo(Variable.Bernoulli(conditionalsWetGrass.WetGrassGivenSprinklerAndRain));
                using (Variable.IfNot(rain)) wetGrass.SetTo(Variable.Bernoulli(conditionalsWetGrass.WetGrassGivenSprinklerNoRain));
            }

            using (Variable.IfNot(sprinkler))
            {
                using (Variable.If(rain)) wetGrass.SetTo(Variable.Bernoulli(conditionalsWetGrass.WetGrassGivenNoSprinklerAndRain));
                using (Variable.IfNot(rain)) wetGrass.SetTo(Variable.Bernoulli(conditionalsWetGrass.WetGrassGivenNoSprinklerNoRain));
            }

            // inference 
            var engine = new InferenceEngine(); // inference algorithms such as belief propagation or expectation propagation

            // set observations

            // case 1: we only observe wet grass and determine weather the rain or the sprinkler causes it
            wetGrass.ObservedValue = true;

            var rainGivenWetGrass = engine.Infer(rain);
            var sprinklerGivenWetGrass = engine.Infer(sprinkler);

            Console.WriteLine("P(rain      | grass is wet)=" + rainGivenWetGrass);
            Console.WriteLine("P(sprinkler | grass is wet)=" + sprinklerGivenWetGrass);

            // case 2: we observe wet grass and no clouds and determine the cause
            cloudy.ObservedValue = false;

            var rainGivenWetGrassNotCloudy = engine.Infer(rain);
            var sprinklerGivenWetGrassNotCLoudy = engine.Infer(sprinkler);

            Console.WriteLine("P(rain      | grass is wet, not cloudy)=" + rainGivenWetGrassNotCloudy);
            Console.WriteLine("P(sprinkler | grass is wet, not cloudy)=" + sprinklerGivenWetGrassNotCLoudy);

            // case 3: we observe wet grass and rain
            // explaining away case to show that rain and sprinkler are conditionally dependent given wetGrass

            cloudy.ClearObservedValue(); // clear previously observed value for this experiment 

            wetGrass.ObservedValue = true;
            var sprinklerGivenWetGrassNoInfo = engine.Infer(sprinkler);

            rain.ObservedValue = true;
            var sprinklerGivenWetGrassAndRain = engine.Infer(sprinkler);

            Console.WriteLine("P(sprinkler      | grass is wet)=" + sprinklerGivenWetGrassNoInfo);
            Console.WriteLine("P(sprinkler | grass is wet and rainy)=" + sprinklerGivenWetGrassAndRain);

        }
    }
}
