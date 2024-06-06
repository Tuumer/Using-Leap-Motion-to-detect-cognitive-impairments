using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Barracuda;
using System.IO;
using System.Globalization;

public class DataTransfer : MonoBehaviour
{
    public static float score_tmt_a;
    public static float score_tmt_b;
    public static float score_bell;
    public static float score_cdt;
    public static float score_line;

    public static float time_tmt_a;
    public static float time_tmt_b;
    public static float time_bell;
    public static float time_cdt;
    public static float time_line;

    public static bool state_tmt_a;
    public static bool state_tmt_b;
    public static bool state_line;
    public static bool state_cdt;
    public static bool state_bell;

    public static string handoTrail;

    public static bool all_test_finished;

    public static float prediction;

    //In the result page
    public TextMeshProUGUI text_score_line;
    public TextMeshProUGUI text_time_line;
    public TextMeshProUGUI text_score_tmt;
    public TextMeshProUGUI text_time_tmt;
    public TextMeshProUGUI text_score_bell;
    public TextMeshProUGUI text_time_bell;
    public TextMeshProUGUI text_score_cdt;
    public TextMeshProUGUI text_time_cdt;

    public TextMeshProUGUI text_acc_line;
    public TextMeshProUGUI text_acc_tmt;
    public TextMeshProUGUI text_acc_bell;
    public TextMeshProUGUI text_acc_cdt;


    //In the second menu
    public TextMeshProUGUI text_score_line_secondary;
    public TextMeshProUGUI text_time_line_secondary;
    public TextMeshProUGUI text_score_tmt_secondary;
    public TextMeshProUGUI text_time_tmt_secondary;
    public TextMeshProUGUI text_score_bell_secondary;
    public TextMeshProUGUI text_time_bell_secondary;
    public TextMeshProUGUI text_score_cdt_secondary;
    public TextMeshProUGUI text_time_cdt_secondary;

    public TextMeshProUGUI text_acc_line_secondary;
    public TextMeshProUGUI text_acc_tmt_secondary;
    public TextMeshProUGUI text_acc_bell_secondary;
    public TextMeshProUGUI text_acc_cdt_secondary;

    public TextMeshProUGUI text_prediction;
    public TextMeshProUGUI text_advise;

    void Start()
    {

        
    }

    void Update()
    {
        text_score_line.text=$"{score_line}";
        text_time_line.text=$"{time_line}";
        text_acc_line.text=$"{score_line}%";

        text_score_tmt.text= $"{score_tmt_a+score_tmt_b}";
        text_time_tmt.text=$"{(time_tmt_a+time_tmt_b)}";
        text_acc_tmt.text=$"{(score_tmt_a+score_tmt_b)/(50)*100}%";

        text_score_bell.text=$"{score_bell}";
        text_time_bell.text=$"{time_bell}";
        text_acc_bell.text=$"{Mathf.Round(score_bell/35*100)}%";

        text_score_cdt.text=$"{score_cdt}";
        text_time_cdt.text=$"{time_cdt}";
        text_acc_cdt.text=$"{score_cdt/10*100}%";

        //Secondary menu
        text_score_line_secondary.text=$"{score_line}";
        text_time_line_secondary.text=$"{time_line}";
        text_acc_line_secondary.text=$"{score_line}%";

        text_score_tmt_secondary.text= $"{score_tmt_a+score_tmt_b}";
        text_time_tmt_secondary.text=$"{(time_tmt_a+time_tmt_b)}";
        text_acc_tmt_secondary.text=$"{(score_tmt_a+score_tmt_b)/(50)*100}%";

        text_score_bell_secondary.text=$"{score_bell}";
        text_time_bell_secondary.text=$"{time_bell}";
        text_acc_bell_secondary.text=$"{Mathf.Round(score_bell/35*100)}%";

        text_score_cdt_secondary.text=$"{score_cdt}";
        text_time_cdt_secondary.text=$"{time_cdt}";
        text_acc_cdt_secondary.text=$"{score_cdt/10*100}%";


        // if(all_test_finished)


    }

    public void pass_to_file(){

        float[] numberInput = new float[14];

        if(DataManagement.gender=="Female") numberInput[0] = 0;
        else numberInput[1]=1;

        numberInput[0]= DataManagement.age;
        numberInput[2]=  DataTransfer.score_tmt_a + DataTransfer.score_tmt_b;
        numberInput[3]=   DataTransfer.score_cdt;
        numberInput[4]=    DataTransfer.score_bell;
        numberInput[5]=    DataTransfer.score_line;
        numberInput[6]=    DataTransfer.time_tmt_a + DataTransfer.time_tmt_b;
        numberInput[7]=   DataTransfer.time_line; 
        numberInput[8]=  LineFollowingGame.meanX;
        numberInput[9]=  LineFollowingGame.sdX;
        numberInput[10]=  LineFollowingGame.meanY;
        numberInput[11]=  LineFollowingGame.sdY;
        numberInput[12]=  LineFollowingGame.meanZ;
        numberInput[13]=  LineFollowingGame.sdZ;

        // numberInput[0] = 34;
        // numberInput[1] = 0;
        // numberInput[2] = 0;
        // numberInput[3] = 2;
        // numberInput[4] = 14;
        // numberInput[5] = 36;
        // numberInput[6] = 95;
        // numberInput[7] = 56;
        // numberInput[8] = 87;
        // numberInput[9] = 9;
        // numberInput[10] = -0.019950f;
        // numberInput[11] = 0.258664f;
        // numberInput[12] = -0.043977f;
        // numberInput[13] = 0.004848f;
        // numberInput[14] = 0.543692f;
        // numberInput[15] = 0.006484f;


       string filePath = Path.Combine(Application.dataPath,"T_Python", "numberInput.txt");

        Debug.Log("Путь: "+ filePath);
        using (StreamWriter writer = new StreamWriter(filePath))
        {

            Debug.Log("Hey-hey!");
            foreach (float number in numberInput)
            {
                // writer.WriteLine(number);
                writer.WriteLine(number.ToString(CultureInfo.InvariantCulture));
            }
        }

    }

    public void load_class(){

        string filePath = Path.Combine(Application.dataPath,"T_Python", "status_prediction.txt");

        if (File.Exists(filePath))
        {
            string predictionString = File.ReadAllText(filePath);
            if (float.TryParse(predictionString, NumberStyles.Float, CultureInfo.InvariantCulture, out prediction))
            {
                Debug.Log("Loaded prediction: " + prediction);

                if(text_prediction!=null&&text_advise!=null){

                    if(prediction == 0){

                        text_prediction.text = "Normal Cognitive";
                        text_advise.text = "Good job! Stay healthy.";

                    }
                    else if( prediction == 1){

                        text_prediction.text = "Mild Cognitive Impairments";
                        text_advise.text = "You should take an appointment to the doctor";

                    }
                    else{

                        text_prediction.text = "Alzheimer's Disease";
                        text_advise.text = "Consult a doctor as soon as possible and take preventive measures!";

                    }
                }
                 
            }
            else
            {
                Debug.LogError("Failed to parse prediction.");
            }
        }
        else
        {
            Debug.LogError("Prediction file not found.");
        }

    }

    public void reset_results(){

        state_tmt_a = false;
        state_tmt_b = false;
        state_cdt = false;
        state_bell = false;
        state_line = false;
        all_test_finished = false;
        
    }


}
