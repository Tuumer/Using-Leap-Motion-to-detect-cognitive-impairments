using UnityEngine;
using Unity.Barracuda;
using System.IO;

public class CNN : MonoBehaviour
{
    public NNModel modelAsset;
    private Model runtimeModel;
    private IWorker worker;
    string outputFolder = "screenshots/cropped_images";

    float[] numberClasses = new float[8];
    private int[] expectedClasses = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };

    private float startTime;
    private float duration;

    void Start(){

        startTime = Time.time;

    }
    public void cnnStart()
    {
        for (int i = 0; i < numberClasses.Length; i++)
        {
            Debug.Log($"Before {i}: " + numberClasses[i]);
        }

        runtimeModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, runtimeModel);

        Debug.Log("Starting with state_cdt: " + DataTransfer.state_cdt);

        Predict();
    }

    public void Predict()
    {
        if (DataTransfer.state_cdt)
        {
            for (int i = 0; i < 8; i++)
            {
                string imagePath = Path.Combine(Application.dataPath, outputFolder, $"img{i + 1}.png");

                Debug.Log($"Looking for image at path: {imagePath}");

                if (File.Exists(imagePath))
                {
                    Texture2D imageTexture = LoadTexture(imagePath);
                    Tensor inputTensor = TransformImageToTensor(imageTexture);

                    if (inputTensor == null)
                    {
                        Debug.LogError($"Failed to create input tensor for image at path: {imagePath}");
                        continue;
                    }

                    worker.Execute(inputTensor);

                    Tensor outputTensor = worker.PeekOutput();
                    int predictedClass = GetPredictedClass(outputTensor);

                    numberClasses[i] = predictedClass;

                    Debug.Log($"Predicted Class for Image {i + 1}: {predictedClass}");

                    inputTensor.Dispose();
                    outputTensor.Dispose();
                }
                else
                {
                    Debug.LogError($"Image not found at path: {imagePath}");
                }
            }

            // Log the predictions for debugging
            Debug.Log("Predicted Classes: " + string.Join(", ", numberClasses));

            CalculateScore();
        }
    }

    Texture2D LoadTexture(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(64, 64); // Set the correct size directly
        texture.LoadImage(fileData);
        return texture;
    }

    Tensor TransformImageToTensor(Texture2D texture)
    {
        // Assuming the model expects 3-channel 64x64 images
        int width = texture.width;
        int height = texture.height;
        int channels = 3; // Assuming RGB channels

        if (width != 64 || height != 64)
        {
            Debug.LogError($"Texture dimensions are not 64x64. Actual dimensions: {width}x{height}");
            return null;
        }

        float[] floatValues = new float[channels * width * height];
        Color32[] pixels = texture.GetPixels32();

        for (int i = 0; i < pixels.Length; i++)
        {
            floatValues[i * 3 + 0] = pixels[i].r / 255.0f;
            floatValues[i * 3 + 1] = pixels[i].g / 255.0f;
            floatValues[i * 3 + 2] = pixels[i].b / 255.0f;
        }

        return new Tensor(1, height, width, channels, floatValues);
    }

    int GetPredictedClass(Tensor outputTensor)
    {
        float maxConfidence = float.MinValue;
        int predictedClass = -1;
        for (int i = 0; i < outputTensor.length; i++)
        {
            if (outputTensor[i] > maxConfidence)
            {
                maxConfidence = outputTensor[i];
                predictedClass = i;
            }
        }
        return predictedClass;
    }
    
    private void CalculateScore()
    {
        int correctPredictions = 0;

        for (int i = 0; i < numberClasses.Length; i++)
        {
            if (numberClasses[i] == expectedClasses[i])
            {
                correctPredictions++;
            }
        }

        float score = (float)correctPredictions / expectedClasses.Length * 100;
        duration = Mathf.Round((Time.time-startTime) * 100f) / 100f;

        Debug.Log($"Score: {score}% ({correctPredictions}/{expectedClasses.Length} correct predictions)");

        DataTransfer.score_cdt = correctPredictions;
        DataTransfer.time_cdt = duration;
    }

    void OnDestroy()
    {
        worker.Dispose();
    }
}
