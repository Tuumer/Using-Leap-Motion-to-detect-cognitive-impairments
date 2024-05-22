using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDT_Result : MonoBehaviour
{
    private MouseDraw mouseDraw;
    private float hour;
    private float minute;

    void Start()
    {
        mouseDraw = FindObjectOfType<MouseDraw>();

        hour = mouseDraw.GetHourResult();
        minute = mouseDraw.GetMinuteResult();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
