using System;

namespace SkillsAssessment
{

    public class PriorProbs
    {
        public double C_sharp { get; set; }
        public double Sql { get; set; }
    }

    public class ConditionalVariablesQuestion1
    {
        public double Correct1GivenC_sharp { get; set; }
        public double Wrong1GivenC_sharp { get; set; }
    }

    public class ConditionalVariablesQuestion2
    {
        public double Correct2GivenSql { get; set; }
        public double Wrong2Given2Sql { get; set; }
    }

    public class ConditionalVariablesQuestion3
    {
        public double Correct3GivenBothSkills { get; set; }
        public double Wrong3GivenBothSkills { get; set; }
    }

}
