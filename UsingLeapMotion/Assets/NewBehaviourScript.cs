using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using Unity.Barracuda;

public class sampleCodeSnippet : MonoBehaviour
{
    [SerializeField]
    private RenderTexture inputImage;
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
        // Convert the RenderTexture to a Tensor
        Texture2D texture = new Texture2D(inputImage.width, inputImage.height, TextureFormat.RGB24, false);
        RenderTexture.active = inputImage;
        texture.ReadPixels(new Rect(0, 0, inputImage.width, inputImage.height), 0, 0);
        texture.Apply();

        using (Tensor inputTensor = new Tensor(texture, channels: 3))  // Create tensor from Texture2D
        {
            worker.Execute(inputTensor);

            Tensor outputTensor = worker.PeekOutput(outputLayerName);
            outputPrediction.text = outputTensor[0].ToString();
        }

        // Clean up
        RenderTexture.active = null;
        Destroy(texture);
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