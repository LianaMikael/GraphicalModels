using System;
using Microsoft.ML.Probabilistic.Models;

namespace SkillsAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            // define the model 
            var priors = new PriorProbs { C_sharp = 0.5, Sql = 0.5 };

            var conditionalQuestion1 = new ConditionalVariablesQuestion1
            {
                Correct1GivenC_sharp = 0.9,
                Wrong1GivenC_sharp = 0.2
            };

            var conditionalQuestion2 = new ConditionalVariablesQuestion2
            {
                Correct2GivenSql = 0.9,
                Wrong2Given2Sql = 0.2
            };

            var conditionalQuestion3 = new ConditionalVariablesQuestion3
            {
                Correct3GivenBothSkills = 0.9,
                Wrong3GivenBothSkills = 0.2
            };

            var c_sharp = Variable.Bernoulli(priors.C_sharp).Named("C_sharp=True");
            var sql = Variable.Bernoulli(priors.Sql).Named("Sql=True");
            var bothSkills = c_sharp & sql;

            var correct1 = Variable.New<bool>().Named("correct1=True");
            var correct2 = Variable.New<bool>().Named("correct2=True");
            var correct3 = Variable.New<bool>().Named("correct3=True");

            // set conditionals
            using (Variable.If(c_sharp)) correct1.SetTo(Variable.Bernoulli(conditionalQuestion1.Correct1GivenC_sharp));
            using (Variable.IfNot(c_sharp)) correct1.SetTo(Variable.Bernoulli(conditionalQuestion1.Wrong1GivenC_sharp));

            using (Variable.If(sql)) correct2.SetTo(Variable.Bernoulli(conditionalQuestion2.Correct2GivenSql));
            using (Variable.IfNot(sql)) correct2.SetTo(Variable.Bernoulli(conditionalQuestion2.Wrong2Given2Sql));

            using (Variable.If(bothSkills)) correct3.SetTo(Variable.Bernoulli(conditionalQuestion3.Correct3GivenBothSkills));
            using (Variable.IfNot(bothSkills)) correct3.SetTo(Variable.Bernoulli(conditionalQuestion3.Wrong3GivenBothSkills));

            // set observations and perform inference
            correct1.ObservedValue = true;
            correct2.ObservedValue = false;
            correct3.ObservedValue = false;

            var inference = new Inference { Engine = new InferenceEngine(), C_sharp = c_sharp, Sql = sql };

            inference.PerformInference();

            Console.WriteLine("P(c_sharp | T, F, F)=" + inference.Posteriors.C_sharp);
            Console.WriteLine("P(sql     | T, F, F)=" + inference.Posteriors.Sql);
            Console.WriteLine("");

            correct1.ObservedValue = false;

            inference.PerformInference();

            Console.WriteLine("P(c_sharp | F, F, F)=" + inference.Posteriors.C_sharp);
            Console.WriteLine("P(sql     | F, F, F)=" + inference.Posteriors.Sql);
            Console.WriteLine("");

            correct2.ObservedValue = true;

            inference.PerformInference();

            Console.WriteLine("P(c_sharp | F, T, F)=" + inference.Posteriors.C_sharp);
            Console.WriteLine("P(sql     | F, T, F)=" + inference.Posteriors.Sql);
            Console.WriteLine("");

            correct1.ObservedValue = true;

            inference.PerformInference();

            Console.WriteLine("P(c_sharp | T, T, F)=" + inference.Posteriors.C_sharp);
            Console.WriteLine("P(sql     | T, T, F)=" + inference.Posteriors.Sql);
            Console.WriteLine("");

            correct1.ObservedValue = false;
            correct2.ObservedValue = false;
            correct3.ObservedValue = true;

            inference.PerformInference();

            Console.WriteLine("P(c_sharp | F, F, T)=" + inference.Posteriors.C_sharp);
            Console.WriteLine("P(sql     | F, F, T)=" + inference.Posteriors.Sql);
            Console.WriteLine("");

            correct1.ObservedValue = true;

            inference.PerformInference();

            Console.WriteLine("P(c_sharp | T, F, T)=" + inference.Posteriors.C_sharp);
            Console.WriteLine("P(sql     | T, F, T)=" + inference.Posteriors.Sql);
            Console.WriteLine("");

            correct1.ObservedValue = false;
            correct2.ObservedValue = true;

            inference.PerformInference();

            Console.WriteLine("P(c_sharp | F, T, T)=" + inference.Posteriors.C_sharp);
            Console.WriteLine("P(sql     | F, T, T)=" + inference.Posteriors.Sql);
            Console.WriteLine("");

            correct1.ObservedValue = true;

            inference.PerformInference();

            Console.WriteLine("P(c_sharp | T, T, T)=" + inference.Posteriors.C_sharp);
            Console.WriteLine("P(sql     | T, T, T)=" + inference.Posteriors.Sql);
            Console.WriteLine("");
        }
    }
}
