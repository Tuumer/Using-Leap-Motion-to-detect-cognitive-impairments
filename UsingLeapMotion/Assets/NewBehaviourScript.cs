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

        // Check if the texture is readable
        if (!originalImage.isReadable)
        {
            UnityEngine.Debug.LogError("Input image is not readable.");
            return null;
        }

        int width = 224;  // Example width, replace with your model's expected width
        int height = 224; // Example height, replace with your model's expected height

        // Resize the image
        Texture2D resizedImage = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Check if the resizing operation was successful
        if (!Graphics.ConvertTexture(originalImage, resizedImage))
        {
            UnityEngine.Debug.LogError("Failed to convert texture.");
            return null;
        }

        // Convert the resized image to a float array
        Color[] resizedPixels = resizedImage.GetPixels();
        float[] floatValues = new float[resizedPixels.Length * 3];
        for (int i = 0; i < resizedPixels.Length; i++)
        {
            Color pixel = resizedPixels[i];
            floatValues[i * 3 + 0] = pixel.r;
            floatValues[i * 3 + 1] = pixel.g;
            floatValues[i * 3 + 2] = pixel.b;
        }

        // Create tensor from float array
        Tensor inputTensor = new Tensor(1, height, width, 3, floatValues);

        return inputTensor;
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }
}
