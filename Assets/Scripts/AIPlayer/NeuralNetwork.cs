using System.Globalization;
using System.Collections.Generic;

public class NeuralNetwork
{
    List<NeuronLayer> layers = new List<NeuronLayer>();

    public NeuralNetwork(string geneticCode)
    {
        layers.Add(new NeuronLayer(2, null)); //output
        layers.Add(new NeuronLayer(100, layers[0])); //hidden
        layers.Add(new NeuronLayer(100, layers[1])); //input

        string[] weights = geneticCode.Split('_');
        int indexOfWeight = 0;
        for (int i = 0; i < 3; i++)
        {
            NeuronLayer neuronLayer = layers[2 - i];
            List<Neuron> neuronsInLayer = neuronLayer.getNeurons();
            for (int j = 0; j < neuronsInLayer.Count; j++)
            {
                Neuron neuron = neuronsInLayer[j];
                for (int k = 0; k < neuron.numberOfConnections(); k++)
                {
                    string weight = weights[indexOfWeight];
                    float weightFloat = float.Parse(weight, CultureInfo.InvariantCulture.NumberFormat);
                    neuron.setWeight(k, weightFloat);
                    indexOfWeight++;
                }
            }
        }
    }

    public float[] execute(float[] inputs) {
        float[] output = new float[2];
        List<Neuron> inputNeurons = layers[2].getNeurons();
        for (int i = 0; i < inputNeurons.Count; i++)
        {
            inputNeurons[i].input(inputs[i]);
            inputNeurons[i].output();
        }
        List<Neuron> hiddenNeurons1 = layers[1].getNeurons();
        for (int i = 0; i < hiddenNeurons1.Count; i++)
        {
            hiddenNeurons1[i].output();
        }
        List<Neuron> outputNeurons = layers[0].getNeurons();
        for (int i = 0; i < outputNeurons.Count; i++)
        {
            output[i] = outputNeurons[i].getAddedSignal();
        }
        return output;
    }
}
