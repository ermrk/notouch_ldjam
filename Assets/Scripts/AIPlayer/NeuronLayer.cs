using System.Collections.Generic;

public class NeuronLayer
{
    List<Neuron> neurons = new List<Neuron>();

    public NeuronLayer(int numberOfNeurons, NeuronLayer nextLayer) {
        for (int i = 0; i < numberOfNeurons; i++) {
            Neuron neuron = new Neuron();
            neurons.Add(neuron);
            if (nextLayer != null) {
                List<Neuron> nextLayerArrayList = nextLayer.getNeurons();
                for (int j = 0; j < nextLayerArrayList.Count; j++) {
                    neuron.registerOutput(nextLayerArrayList[j]);
                }
            }
        }
    }

    public List<Neuron> getNeurons() {
        return neurons;
    }
}
