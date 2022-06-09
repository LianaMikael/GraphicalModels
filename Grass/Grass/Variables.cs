using System;
namespace Grass
{
    public class PriorProbs
    {
        // prior probability of cloudy weather
        public double Cloudy { get; set; }
    }
    
    public class ConditionalVariablesSprinkler
    {
        // probabilities of sprinkler being on conditioned on cloudy weather
        public double SprinklerGivenCloudy { get; set; }
        public double SprinklerGivenNotCloudy { get; set; }
    }

    public class ConditionalVariablesRain
    {
        // probabilities of rain conditioned on cloudy weather
        public double RainGivenCloudy { get; set; }
        public double RainGivenNotCloudy { get; set; }
    }

    public class ConditionalVariablesGrass
    {
        // probabilities of grass being wet conditioned on sprinkler and rain
        public double WetGrassGivenSprinklerAndRain { get; set; }
        public double WetGrassGivenSprinklerNoRain { get; set; }
        public double WetGrassGivenNoSprinklerAndRain { get; set; }
        public double WetGrassGivenNoSprinklerNoRain { get; set; }
    }
}
