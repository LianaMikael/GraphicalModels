using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace SkillAssessmentAdvanced
{
    public class DataCollection
    {
        public string FileName { get; set; }
        public string QuestionSkillsFile { get; set; }
        public int PeopleNum { get; set; }
        public int SkillNum { get; set; }
        public int QuestionNum { get; set; }

        public int[][] Data { get; set; }
        public string[][] SkillsGroundTruth { get; set; }
        public int[] AnswersGroundTruth { get; set; }
        public int[][] SkillsNeeded { get; set; }

        public bool[][] AnswersIsCorrect { get; set; }
        public int[] SkillNumPerQuestion { get; set; }

        public void CollectData()
        {
            this.Data = new int[PeopleNum][];
            this.SkillsGroundTruth = new string[PeopleNum][];
            this.AnswersGroundTruth = new int[this.QuestionNum];


            using (TextFieldParser csvParser = new TextFieldParser(this.FileName))
            {
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.ReadLine();

                int i = 0;
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();

                    if (i != 0)
                    {
                        // Slice the row to extract people's answers and skills' ground truth, convert to ints.
                        this.Data[i - 1] = ConvertToInts(fields[(this.SkillNum + 1)..(this.SkillNum + this.QuestionNum + 1)]);
                        this.SkillsGroundTruth[i-1] = fields[1..(this.SkillNum + 1)];
                    }
                    else
                    {
                        // Slice the row to extract the ground truth answers, convert to ints.
                        int[] fields_int = ConvertToInts(fields[(this.SkillNum + 1)..(this.SkillNum + this.QuestionNum + 1)]); 
                        this.AnswersGroundTruth = fields_int;
                    }

                    i++;
                }
            }
        }

        private int[] ConvertToInts(string[] array)
        {
            return Array.ConvertAll(array, s => int.Parse(s));
        }

        public void CheckAnswers()
        {
            // Constructs an boolean array of isCorrect answers for each question for each person.
            this.AnswersIsCorrect = new bool[PeopleNum][];
            for (int i = 0; i < PeopleNum; i++)
            {
                var answersPerPerson = new bool[QuestionNum];
                for (int j = 0; j < QuestionNum; j++)
                {
                    var isEqual = this.Data[i][j] == this.AnswersGroundTruth[j];
                    answersPerPerson[j] = isEqual;
                }
                this.AnswersIsCorrect[i] = answersPerPerson;
            }
        }

        public void CollectSkillsNeeded()
        {
            // Constructs an array of arrays of skills needed for each question.
            this.SkillsNeeded = new int[this.QuestionNum][];
            this.SkillNumPerQuestion = new int[this.QuestionNum];

            using (TextFieldParser csvParser = new TextFieldParser(this.QuestionSkillsFile))
            {
                csvParser.SetDelimiters(new string[] { "," });

                int i = 0;
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    var fields_int = new List<int>();

                    // Convert True/False string answers to int corresponding to index of skills,
                    // compute the number of skills needed.
                    for (int k = 0; k < fields.Length; k++)
                    {
                        if (fields[k] == "True")
                        {
                            fields_int.Add(k);
                            SkillNumPerQuestion[i]++;
                        }
                            
                    }

                    this.SkillsNeeded[i] = fields_int.ToArray();
                    i++;

                }
             
            }
        }

    }
}
