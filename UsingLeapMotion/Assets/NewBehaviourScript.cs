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
        // Load the ONNX model
        runtimeModel = ModelLoader.Load(onnxAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
        outputLayerName = runtimeModel.outputs[runtimeModel.outputs.Count - 1];  // Get the name of the last output layer
    }

    public void Predict()
    {
        // Preprocess the input image
        Tensor inputTensor = PreprocessImage(inputImage);

        if (inputTensor == null)
        {
            UnityEngine.Debug.LogError("Failed to preprocess the image.");
            return;
        }

        worker.Execute(inputTensor);

        Tensor outputTensor = worker.PeekOutput(outputLayerName);
        outputPrediction.text = outputTensor[0].ToString();

        // Dispose the tensors to free resources
        inputTensor.Dispose();
        outputTensor.Dispose();
    }

    // Preprocess the image to match the model's expected input dimensions and format
    private Tensor PreprocessImage(Texture2D originalImage)
    {
        if (originalImage == null)
        {
            UnityEngine.Debug.LogError("Input image is null.");
            return null;
        }

        // Convert the Texture2D to a float array
        Color[] pixels = originalImage.GetPixels();
        float[] floatValues = new float[pixels.Length * 3];
        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            floatValues[i * 3 + 0] = pixel.r;
            floatValues[i * 3 + 1] = pixel.g;
            floatValues[i * 3 + 2] = pixel.b;
        }

        // Create a tensor from the float array
        Tensor inputTensor = new Tensor(1, originalImage.height, originalImage.width, 3, floatValues);

        return inputTensor;
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }
}
