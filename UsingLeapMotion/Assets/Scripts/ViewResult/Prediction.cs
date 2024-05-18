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

    public TextMeshProUGUI inputValue;
    public TextMeshProUGUI predictionOutput;

    void Start()
    {
        runtimeModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, runtimeModel);
    }

    public void Predict()
    {
    
        float[] numberInput = new float[3];

        string[] inputs = inputValue.text.Split(',');

        for (int i = 0; i < inputs.Length; i++)
        {
            Debug.Log($"Input {i}: '{inputs[i]}'");
        }

        if (inputs.Length == 3)
        {
            bool parseSuccess = true;
            for (int i = 0; i < 3; i++)
            {
                string cleanedInput = Regex.Replace(inputs[i].Trim(), @"[^\d.-]", "");
                if (string.IsNullOrEmpty(cleanedInput) || !float.TryParse(cleanedInput, NumberStyles.Float, CultureInfo.InvariantCulture, out numberInput[i]))
                {
                    Debug.LogError($"Failed to parse input {i}: '{inputs[i]}' (cleaned: '{cleanedInput}')");
                    parseSuccess = false;
                }
                else
                {
                    Debug.Log($"Parsed input {i} successfully: {numberInput[i]}");
                }
            }

            if (parseSuccess)
            {
                Tensor inputTensor = new Tensor(1, 3, numberInput);

                worker.Execute(inputTensor);

                Tensor outputTensor = worker.PeekOutput();

                float prediction = outputTensor[0];

                predictionOutput.text = prediction.ToString();

                inputTensor.Dispose();
                outputTensor.Dispose();
            }
            else
            {
                predictionOutput.text = "Invalid input: failed to parse one or more inputs.";
            }
        }
        else
        {
            predictionOutput.text = "Invalid input: please provide exactly three comma-separated numbers.";
        }
    }

    void OnDestroy()
    {
        worker.Dispose();
    }
}
