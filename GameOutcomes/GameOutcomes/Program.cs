using System;
using Microsoft.ML.Probabilistic.Models;
using MathNet.Numerics.Distributions;
using System.Collections.Generic;
using System.Linq;

namespace GameOutcomes
{
    class Program
    {
        static void Main(string[] args)
        {
             
            Console.WriteLine("Self-assessment 3.1 ");
            Console.WriteLine("3.1.1");

            (double[] samples, double set1Perc, double set2Perc, double set3Perc) = ComputeSamplePercentages(0, 1, 10000);

            Console.WriteLine("Percentage of samples between -1 and 1:" + set1Perc);
            Console.WriteLine("Percentage of samples between -2 and 2:" + set2Perc);
            Console.WriteLine("Percentage of samples between -3 and 3:" + set3Perc);

            Console.WriteLine("3.1.3");
            (double mean, double variance, double std) = GetStatistics(samples);
            Console.WriteLine("Mean:              " + mean);
            Console.WriteLine("Variance:          " + variance);
            Console.WriteLine("Standard Devation: " + std);

            GameModel gameModel = new GameModel();

            Console.WriteLine("3.1.5");
            Console.WriteLine("P(JWins|JPerf, FPerf)=" + gameModel.gameOutcome);

            Console.WriteLine("Self-assessment 3.4 ");
            Console.WriteLine("3.3.4");
            SkillModel JWinsmodel = new SkillModel(true);

            Console.WriteLine("P(Jskill|JWins)=" + JWinsmodel.JSkillPredicted);
            Console.WriteLine("P(Fskill|JWins)=" + JWinsmodel.FSkillPredicted);

            SkillModel FWinsmodel = new SkillModel(false);

            Console.WriteLine("P(Jskill|FWins)=" + FWinsmodel.JSkillPredicted);
            Console.WriteLine("P(Fskill|FWins)=" + FWinsmodel.FSkillPredicted);

        }

        private static (double, double, double) GetStatistics(double[] samples)
        {
            double mean = (double) samples.Sum() / samples.Length;
            double variance = 0;
            foreach (double sample in samples)
            {
                variance += Math.Pow((sample - mean), 2);
            }
            variance /= samples.Length;

            double std = Math.Sqrt(variance);

            return (mean, variance, std);
        }

        private static (double[], double, double, double) ComputeSamplePercentages(double mean, double std, int numSamples)
        {
            int set1Count = 0;
            int set2Count = 0;
            int set3Count = 0;

            double[] samples = new double[numSamples];

            for (int i = 0; i < numSamples; i++)
            {
                double sample = SampleGaussian(mean, std);
                samples[i] = sample;
                if (sample > -1 && sample < 1)
                    set1Count++;
                if (sample > -2 && sample < 2)
                    set2Count++;
                if (sample > -3 && sample < 3)
                    set3Count++;
            }

            return (samples, set1Count / (double)numSamples, set2Count / (double)numSamples, set3Count / (double)numSamples);
        }

        private static double SampleGaussian(double mean, double std)
        {
            MathNet.Numerics.Distributions.Normal normalDist = new Normal(mean, std);
            return normalDist.Sample();
        }

    }
}
