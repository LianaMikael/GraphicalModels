using System;
using Microsoft.ML.Probabilistic.Models;

namespace SkillAssessmentAdvanced
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataCollection = new DataCollection {
                FileName = "/Users/liana/Desktop/MBML/GraphicalModels/SkillAssessmentAdvanced/LearningSkills_6LearningTheGuessProbabilitiesComparison_Experiments-Original-Inputs-RawResponsesAsDictionary.csv",
                QuestionSkillsFile = "/Users/liana/Desktop/MBML/GraphicalModels/SkillAssessmentAdvanced/LearningSkills_6LearningTheGuessProbabilitiesComparison_Experiments-Original-Inputs-Quiz-SkillsQuestionsMask.csv",
                PeopleNum = 22,
                SkillNum = 7,
                QuestionNum = 48
            };

            dataCollection.CollectData();
            dataCollection.CollectSkillsNeeded();
            dataCollection.CheckAnswers();
         
            var skillsGroundTruth = dataCollection.SkillsGroundTruth;
            var skillsNeeded = dataCollection.SkillsNeeded;
            var answersIsCorrect = dataCollection.AnswersIsCorrect;
            var skillNumPerQuestion = dataCollection.SkillNumPerQuestion;

            var realDataModel = new RealDataModel {
                SkillsNumInt = 7,
                QuestionNumInt = 48,
                PeopleNumInt = 22,
                ProbCorrect = 0.9,
                ProbWrong = 0.2,
                SkillsPrior = 0.5,
                SkillsNeeded = skillsNeeded,
                IsCorrect = answersIsCorrect,
                SkillNumPerQuestion = skillNumPerQuestion,
            };

            realDataModel.ConstructModel();

            var inference = new Inference { Engine = new InferenceEngine(), Skills = realDataModel.Skills};
            inference.PerformInference();

            Console.WriteLine("Skills inferred: " + inference.SkillsPosterior);

        }

    }
}
