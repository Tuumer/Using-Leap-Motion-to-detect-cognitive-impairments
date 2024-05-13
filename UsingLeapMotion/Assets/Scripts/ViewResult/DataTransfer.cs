using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


    void Start()
    {

        
    }

    void Update()
    {
        text_score_line.text=$"{score_line}";
        text_time_line.text=$"{time_line}";
        text_acc_line.text="90%";

        text_score_tmt.text= $"{score_tmt_a+score_tmt_b}";
        text_time_tmt.text=$"{(time_tmt_a+time_tmt_b)}";
        text_acc_tmt.text="90%";

        text_score_bell.text=$"{score_bell}";
        text_time_bell.text=$"{time_bell}";
        text_acc_bell.text="90%";

        text_score_cdt.text=score_cdt.ToString();
        text_time_cdt.text=time_cdt.ToString();
        text_acc_cdt.text="90%";


    }
}
