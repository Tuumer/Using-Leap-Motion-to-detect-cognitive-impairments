using System;
using System.IO;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class CDT_Result : MonoBehaviour
{
    private MouseDraw mouseDraw;

    private float startTime;
    private float duration;

    void Start()
    {
        mouseDraw = FindObjectOfType<MouseDraw>();
        startTime = Time.time;
        
    }

    public void LogSum()
    {
        float hour = mouseDraw.GetHourResult();
        float minute = mouseDraw.GetMinuteResult();
        float hour_score, minute_score;

        if (hour > 3)
        {
            hour_score = 1;
        }
        else { 
            hour_score = 0; 
        }

        if (minute > 3)
        {
            minute_score = 1;
        }
        else
        {
            minute_score = 0;
        }



        float cnn_score = 0;

        string filePath = Path.Combine(UnityEngine.Application.dataPath, "T_Python", "true_count.txt");

        try
        {
            string trueCountString = File.ReadAllText(filePath);
            cnn_score = float.Parse(trueCountString);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Failed to read true_count from file: " + e.Message);
        }

        duration = Mathf.Round((Time.time-startTime) * 100f) / 100f;
        DataTransfer.time_cdt=duration;
        DataTransfer.state_cdt = true;

        float sum = hour_score + minute_score + cnn_score;
        UnityEngine.Debug.Log("CDT Result: " + sum);

        DataTransfer.score_cdt = sum;
    }
}
