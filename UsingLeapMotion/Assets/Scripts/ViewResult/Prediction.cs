using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using TMPro;
using System.Text.RegularExpressions;
using System.Globalization;


public class Predictor : MonoBehaviour
{
    public NNModel modelAsset; 
    private Model runtimeModel;
    private IWorker worker;

    public TextMeshProUGUI predictionOutput;


    public void Predict()
    {

        runtimeModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, runtimeModel);
    
        float[] numberInput = new float[16];

        if(DataManagement.gender=="Female") numberInput[0] = 0;
        else numberInput[1]=1;

        numberInput[0]= DataManagement.age;
        numberInput[2]=  DataTransfer.score_tmt_a + DataTransfer.score_tmt_b;
        numberInput[3]=   DataTransfer.score_cdt;
        numberInput[4]=    DataTransfer.score_bell;
        numberInput[5]=    DataTransfer.score_line;
        numberInput[6]=    DataTransfer.time_tmt_a + DataTransfer.time_tmt_b;
        numberInput[7]=    DataTransfer.time_cdt;
        numberInput[8]=   DataTransfer.time_bell;
        numberInput[9]=   DataTransfer.time_line; 
        numberInput[10]=  LineFollowingGame.meanX;
        numberInput[11]=  LineFollowingGame.sdX;
        numberInput[12]=  LineFollowingGame.meanY;
        numberInput[13]=  LineFollowingGame.sdY;
        numberInput[14]=  LineFollowingGame.meanZ;
        numberInput[15]=  LineFollowingGame.sdZ;

        for(int i =0;i<numberInput.Length;i++){
            Debug.Log(numberInput[i]);
        }
        
        bool parseSuccess = true;
      

        if (parseSuccess)
    {
        
            // Create a 4D tensor with shape (0, 1, 1, 16)
            TensorShape inputShape = new TensorShape(0, 1, 1, 16);
            Tensor inputTensor = new Tensor(inputShape, numberInput);

            // Execute the worker
            worker.Execute(inputTensor);

            // Get the output tensor
            Tensor outputTensor = worker.PeekOutput();

            // Get the prediction from the output tensor
            float prediction = outputTensor[0];

            Debug.Log("Class of prediction: "+ prediction);

            // Display the prediction
            float threshold = 0.5f;

            // Check if the prediction is above the threshold
            if (prediction >= threshold)
            {
                predictionOutput.text = "Alzheimer's (Class 1)";
            }
            else
            {
                predictionOutput.text = "Normal Cognitive (Class 0)";
            }

            // Dispose tensors
            inputTensor.Dispose();
            outputTensor.Dispose();

    }
    else
    {
        Debug.LogError("Invalid input: failed to parse one or more inputs.");
        predictionOutput.text = "Invalid input: failed to parse one or more inputs.";
    }
    }

    void OnDestroy()
    {
        if(worker!=null){
            worker.Dispose();
        }
        
    }
}
