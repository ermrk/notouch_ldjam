using System.Collections.Generic;
using System;

public class Neuron {

    List<float> outputWeights = new List<float>();
    List<Neuron> outputNeurons = new List<Neuron>();

    float addedSignal = 0;

    public void input(float input) {
        addedSignal += input;
    }

    public void output() {
        for (int i = 0; i < outputNeurons.Count; i++) {
            float signalToSend = Sigmoid(addedSignal * outputWeights[i]);
            outputNeurons[i].input(signalToSend);
        }
        addedSignal = 0;
    }

    private float Sigmoid(float x)
    {
        return 2.0f / (1.0f + (float)Math.Exp(-2.0f * x)) - 1.0f;
    }

    public void registerOutput(Neuron neuron) {
        outputNeurons.Add(neuron);
        outputWeights.Add(1.0f);
    }

    public float getAddedSignal() {
        float toReturn = addedSignal;
        addedSignal = 0;
        return Sigmoid(toReturn);
    }

    public void setWeight(int connection, float weight) {
        outputWeights[connection] = weight;
    }

    public int numberOfConnections() {
        return outputWeights.Count;
    }
}
