using UnityEngine;
using Unity.Barracuda;
public class sampleCodeSnippet : MonoBehaviour
{
    public NNModel onnxAsset;
    public Texture2D imageToRecognise;
    private IWorker worker;
    void Start()
    {

        // Create a worker for executing the model
        worker = onnxAsset.CreateWorker();

        using (var input = new Tensor(imageToRecognise, channels: 3))
        {
            // execute neural network with specific input and get results back
            var output = worker.Execute(input).PeekOutput();

            // the following line will access values of the output tensor causing the main thread to block until neural network execution is done
            var indexWithHighestProbability = output[0];

            UnityEngine.Debug.Log($"Image was recognised as class number: " + output[0] + " " + output[1]);
        }
    }

}