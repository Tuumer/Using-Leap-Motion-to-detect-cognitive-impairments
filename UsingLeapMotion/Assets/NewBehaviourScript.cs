using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using Unity.Barracuda;

public class sampleCodeSnippet : MonoBehaviour
{
    [SerializeField]
    private Texture2D inputImage;
    [SerializeField]
    private TMP_Text outputPrediction;
    [SerializeField]
    public NNModel onnxAsset;

    private Model runtimeModel;
    private IWorker worker;
    private string outputLayerName;

    void Start()
    {
        runtimeModel = ModelLoader.Load(onnxAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
        outputLayerName = runtimeModel.outputs[runtimeModel.outputs.Count - 1];  // Get the name of the last output layer
    }

    public void Predict()
    {
        using (Tensor inputTensor = new Tensor(inputImage, channels: 3))  // Create tensor from Texture2D
        {
            worker.Execute(inputTensor);

            Tensor outputTensor = worker.PeekOutput(outputLayerName);
            outputPrediction.text = outputTensor[0].ToString();
        }
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }
}

/*
        // Create a worker for executing the model
        worker = onnxAsset.CreateWorker();

        using (var input = new Tensor(imageToRecognise, channels: 3))
        {
            // execute neural network with specific input and get results back
            var output = worker.Execute(input).PeekOutput();

            // the following line will access values of the output tensor causing the main thread to block until neural network execution is done
            var indexWithHighestProbability = output[0];

            UnityEngine.Debug.Log($"Image was recognised as class number: " + output[0] + " " + output[1]);
*/