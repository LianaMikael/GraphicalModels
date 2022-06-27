using Microsoft.ML.Probabilistic.Models;

namespace SkillAssessmentAdvanced
{
    public class Inference
    {
        public InferenceEngine Engine { get; set; }
        public VariableArray<VariableArray<bool>, bool[][]> Skills { get; set; }
        public object SkillsPosterior  { get; set; }

        public void PerformInference()
        {
            this.SkillsPosterior = Engine.Infer(Skills);
        }

    }

}

