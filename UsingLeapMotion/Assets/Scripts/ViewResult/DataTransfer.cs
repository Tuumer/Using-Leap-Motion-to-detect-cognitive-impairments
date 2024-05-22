using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Barracuda;

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


    }


}
