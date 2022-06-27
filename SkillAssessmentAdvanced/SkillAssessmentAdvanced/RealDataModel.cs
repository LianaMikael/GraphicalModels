using Microsoft.ML.Probabilistic.Models;

namespace SkillAssessmentAdvanced
{
    public class RealDataModel
    {
        public int SkillsNumInt { get; set; }
        public int QuestionNumInt { get; set; }
        public int PeopleNumInt { get; set; }

        public Variable<int> SkillsNum { get; set; }
        public Variable<int> QuestionNum { get; set; }
        public Variable<int> PeopleNum { get; set; }

        public Variable<double> SkillsPrior { get; set; }

        // array of arrays [skills[0], skills[1], skills[2] ... for each question]
        public int[][] SkillsNeeded { get; set; }
        public int[] SkillNumPerQuestion { get; set; }
        public bool[][] IsCorrect { get; set; }

        public double ProbCorrect { get; set; }
        public double ProbWrong { get; set; }

        public VariableArray<VariableArray<bool>, bool[][]> Skills { get; set; }
        public VariableArray<int> SkillNumPerQuestionArray { get; set; }
        public VariableArray<VariableArray<int>, int[][]> SkillsNeededArray { get; set; }
        public VariableArray<VariableArray<bool>, bool[][]> QuestionsIsCorrect { get; set; }

        public void ConstructModel()
        {
            this.SkillsNum = Variable.New<int>().Named("skillsNum");
            this.QuestionNum = Variable.New<int>().Named("questionNum");
            this.PeopleNum = Variable.New<int>().Named("peopleNum");

            var skill = new Range(this.SkillsNum);
            var person = new Range(this.PeopleNum);
            var question = new Range(this.QuestionNum);

            this.SkillNumPerQuestionArray = Variable.Array<int>(question).Named("SkillNumPerQuestionArray");
            this.ConvertSkillNumToArray();

            var questionsSkills = new Range(this.SkillNumPerQuestionArray[question]).Named("questionsSkills");

            this.SkillsNeededArray = Variable.Array(Variable.Array<int>(questionsSkills), question).Named("SkillsNeededArray");
            this.ConvertSkillsNeededToArray();

            this.Skills = Variable.Array(Variable.Array<bool>(skill), person).Named("skills");
            this.Skills[person][skill] = Variable.Bernoulli(SkillsPrior).ForEach(person).ForEach(skill); 

            // array of arrays of question answers for each person (observed)
            this.QuestionsIsCorrect = Variable.Array(Variable.Array<bool>(question), person).Named("IsCorrect");
            this.ConstuctNoisyFactor(person, question);

            this.SetObservations();

        }

        private void SetObservations()
        {
            this.QuestionsIsCorrect.ObservedValue = this.IsCorrect;
            this.SkillsNum.ObservedValue = SkillsNumInt;
            this.QuestionNum.ObservedValue = QuestionNumInt;
            this.PeopleNum.ObservedValue = PeopleNumInt;
        }

        private Variable<bool> AddNoise(VariableArray<bool>  relevantSkills)
        {
            var noisyAllTrue = Variable.New<bool>().Named("NoisyAllTrueGated");

            // has skills is true if person has all relevant skills to the questions
            var hasSkills = Variable.AllTrue(relevantSkills).Named("hasSkills");

            using (Variable.If(hasSkills)) noisyAllTrue.SetTo(Variable.Bernoulli(ProbCorrect));
            using (Variable.IfNot(hasSkills)) noisyAllTrue.SetTo(Variable.Bernoulli(ProbWrong));

            return noisyAllTrue;
        }

        private void ConvertSkillNumToArray()
        {
            for (int i = 0; i < this.SkillNumPerQuestion.Length; i++)
                this.SkillNumPerQuestionArray[i] = this.SkillNumPerQuestion[i];
        }

        private void ConvertSkillsNeededToArray()
        {
            for (int i = 0; i < this.SkillsNeeded.Length; i++)
            {
                var skillsArrayInside = Variable.Array<int>(new Range(this.SkillsNeeded[i].Length));
                for (int j = 0; j < this.SkillsNeeded[i].Length; j++)
                {
                    skillsArrayInside[j] = this.SkillsNeeded[i][j];
                }

                SkillsNeededArray[i] = skillsArrayInside;
            }

        }

        private void ConstuctNoisyFactor(Range person, Range question)
        {
            using (Variable.ForEach(person))
            {
                using (Variable.ForEach(question))
                {
                    var relevantSkills = Variable.Subarray(this.Skills[person], this.SkillsNeededArray[question]).Named("relevantSkills");

                    this.QuestionsIsCorrect[person][question] = AddNoise(relevantSkills);
                }
            }
        }

    }
}
