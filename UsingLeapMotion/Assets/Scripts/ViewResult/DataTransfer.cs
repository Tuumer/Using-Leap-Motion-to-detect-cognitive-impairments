using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataTransfer : MonoBehaviour
{
    public static float score_tmt;
    public static string score_line;
    public static float time_line;
    public static float time_tmt;

    public TextMeshProUGUI text_score_line;
    public TextMeshProUGUI text_time_line;
    public TextMeshProUGUI text_score_tmt;
    public TextMeshProUGUI text_time_tmt;


    void Start()
    {

        
    }

    void Update()
    {
        text_score_line.text=score_line;
        text_time_line.text=time_line.ToString();

        text_score_tmt.text=score_tmt.ToString();
        text_time_tmt.text=time_tmt.ToString();


    }
}
