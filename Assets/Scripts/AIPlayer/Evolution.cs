using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Evolution
{
    List<Individual> generation = new List<Individual>();
    List<float> bestOfLast = new List<float>();

    Random random = new Random();
    AiPlayer aiPlayer;

    int index = 0;

    int sizeOfGeneration = 20;

    int generationNumber = 0;

    float bestOfCurrentGeneration = 0;

    string generationFileName = "Generation.txt";

    string generationBestsFileName = "GenerationBests.txt";

    string generationNumberFileName = "GenerationNumber.txt";

    string bestIndividualFileName = "BestIndividual.txt";

    string bestIndividualScoreFileName = "BestIndividualScore.txt";

    string bestIndividualsInGenerationsFileName = "BestIndividualsInGenerations.txt";

    Individual bestIndividual;


    public Evolution(AiPlayer aiPlayer)
    {
        bestIndividual = new Individual();
        for (int i = 0; i < 6; i++)
        {
            bestOfLast.Add(0);
        }
        this.aiPlayer = aiPlayer;
        try
        {
            string line = "";
            System.IO.StreamReader file = new System.IO.StreamReader(generationFileName);
            while ((line = file.ReadLine()) != null)
            {
                Individual individual = new Individual();
                individual.setCode(line);
                generation.Add(individual);
            }
            file.Close();

            System.IO.StreamReader file2 = new System.IO.StreamReader(generationNumberFileName);
            while ((line = file2.ReadLine()) != null)
            {
                generationNumber = Int32.Parse(line);
            }
            file2.Close();
        }
        catch (Exception e)
        {
            aiPlayer.writeDebug("GENERATING NEW INDIVIDUALS");
            generateFirstGeneration();
        }

        try
        {
            string line;
            System.IO.StreamReader file3 = new System.IO.StreamReader(bestIndividualFileName);
            while ((line = file3.ReadLine()) != null)
            {
                bestIndividual.setCode(line);
            }
            file3.Close();

            System.IO.StreamReader file4 = new System.IO.StreamReader(bestIndividualScoreFileName);
            while ((line = file4.ReadLine()) != null)
            {
                bestIndividual.setScore(Int32.Parse(line));
            }
            file4.Close();
        }
        catch (Exception e) { }

    }

    void generateFirstGeneration()
    {
        for (int i = 0; i <= sizeOfGeneration; i++)
        {
            generation.Add(generateIndividual());
        }
    }

    Individual generateIndividual()
    {
        string code = "";
        for (int i = 0; i < 60 * 60 + 60 * 60 + 60 * 60 + 60 * 60 + 60 * 60 + 60 * 2; i++)
        {
            code += GetRandomNumber(-5.0f, 5.0f) + "_";
        }
        Individual individual = new Individual();
        individual.setCode(code);
        return individual;
    }

    public NeuralNetwork getNextNeuralNetwork()
    {
        if (index >= generation.Count)
        {
            saveGeneration();
            saveGenerationNumber();
            bestOfCurrentGeneration = 0;
            evolve();
            generationNumber++;
            index = 0;
        }
        NeuralNetwork neuralNetwork = new NeuralNetwork(generation[index].getCode());
        index++;
        return neuralNetwork;
    }

    public float GetRandomNumber(float minimum, float maximum)
    {
        return (float)random.NextDouble() * (maximum - minimum) + minimum;
    }

    public void setDistance(float distance)
    {
        generation[index - 1].setScore(distance);
        if (distance > bestOfCurrentGeneration)
        {
            bestOfCurrentGeneration = distance;
        }
        if (distance > bestIndividual.getScore())
        {
            bestIndividual = generation[index - 1];
            saveBestIndividual();
        }
    }

    void evolve()
    {
        generation = generation.OrderByDescending(
            x => x.getScore()
        ).ToList();
        generation = generation.GetRange(0, 6);
        saveGenerationBest(generation[0].getCode());
        saveGenerationBestScore(generation[0].getScore());
        bestOfLast.Add(generation[0].getScore());
        bestOfLast = bestOfLast.GetRange(bestOfLast.Count() - 6, 6);
        int i = 0;
        while (generation.Count <= sizeOfGeneration)
        {
            Individual individual = new Individual();
            individual.setCode(generation[i].getCode());
            generation.Add(individual);
            i = (i + 1) % 6;
        }
        for (int j = 6; j < generation.Count; j++)
        {
            generation[j].mutate();
        }
    }

    public int getGenerationNumber()
    {
        return generationNumber;
    }

    public int getIndex()
    {
        return index;
    }

    public List<float> getBestOfLast()
    {
        return bestOfLast;
    }

    public float getBestOfCurrentGeneration()
    {
        return bestOfCurrentGeneration;
    }

    void saveGeneration()
    {
        StreamWriter sr = File.CreateText(generationFileName);
        foreach (Individual individual in generation)
        {
            sr.WriteLine(individual.getCode());
        }
        sr.Close();
    }

    void saveGenerationBest(string code)
    {
        StreamWriter sr = File.AppendText(generationBestsFileName);
        sr.WriteLine(code);
        sr.WriteLine("");
        sr.Close();
    }

    void saveGenerationNumber()
    {
        StreamWriter sr = File.CreateText(generationNumberFileName);
        sr.WriteLine(generationNumber);
        sr.Close();
    }

    void saveBestIndividual()
    {
        StreamWriter sr = File.CreateText(bestIndividualFileName);
        sr.WriteLine(bestIndividual.getCode());
        sr.Close();

        StreamWriter sr2 = File.CreateText(bestIndividualScoreFileName);
        sr2.WriteLine(bestIndividual.getScore());
        sr2.Close();
    }

    void saveGenerationBestScore(float score)
    {
        StreamWriter sr = File.AppendText(bestIndividualsInGenerationsFileName);
        sr.WriteLine(score);
        sr.Close();
    }
}
