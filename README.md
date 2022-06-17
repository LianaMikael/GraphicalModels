# Inference in Probabilistic Graphical Models

This repo explores inference in Probabilistic Graphical Models using Infer.NET in C#.
[Infer.Net]((https://dotnet.github.io/infer/default.html)) is a framework for performing inference in graphical models. Here we explore several examples following [Infer.NET User Guide](https://dotnet.github.io/infer/userguide/) and [Model-Based Machine Learning book](https://mbmlbook.com/).  

# Getting Started

We will be using Visual Studio. First, follow [this tutorial](https://dotnet.microsoft.com/en-us/learn/dotnet/hello-world-tutorial/intro) to install .NET.
Then create an application directly in Visual Studio or using the terminal:

``` 
dotnet new console -o project
``` 

Now install the following package for using Infer.NET:
``` 
cd project
dotnet add package Microsoft.ML.Probabilistic.Compiler
``` 

# Tutorials and Examples 

## Grass/Rain/Sprinkler Problem

Consider the following example: suppose you go outside and observe that the grass is wet. You know that this can happen if it was raining or if sprinklers were on. The goal is to determine the cause of the wet grass. You can also use other evidence that help you do it. For example, you may also observe that it is cloudy.

To model this, we create a directed graphical model where each node represents one of ther four random variables - cloudy, sprinklers, rain and wet grass. Note that some of variables are observed, while others are hidden; directed esges indicate dependence between the variables. 

The code for this project is provided in Grass folder. 

For more information about this example, see:
- [Theoretical tutorial with grass/rain/sprinkler example](https://www.cs.ubc.ca/~murphyk/Bayes/bnintro.html)
- [Discussion with example where the code is adapted from](https://social.microsoft.com/Forums/en-US/dcffcf8d-fb15-4236-98fd-9d4a5b19e03a/example-of-bayesian-network-migrated-from-communityresearchmicrosoftcom?forum=infer.net)

## Skills Assessment 

This problem is taken from [Model-Based Machine Learning book](https://mbmlbook.com/). Please refer to this book for many great examples and non-technical explanations. 

The code for this project is provided in SkillsAssessment folder.

## Game Outcomes

Here we explore the **Chapter 3. Meeting Your Match** from [Model-Based Machine Learning book](https://mbmlbook.com/). 

The code for exercises from the book are given in GameOutcomes folder. 
  
