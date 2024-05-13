using System.Collections;
using System.Collections.Generic;
using System.IO;
using Leap;
using Leap.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LineFollowingGame : MonoBehaviour
{
    public LeapProvider leapProvider;
    public Camera mainCamera;
    public RectTransform preGeneratedLine;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI coveredDistanceText;
    

    private Hand hand;

    [Tooltip("The Canvas")]
    private Canvas hostCanvas;
    private bool countdownStarted = false;
    private bool gameStarted = false;
    private bool gameEnded = false;

    private float lineStartX;
    private float lineEndX;

    [SerializeField]
    private List<Vector3> handTrail = new List<Vector3>();
    private float startTime;
    private float duration;
    private float accuracy;
    

    private void Start()
    {
        hostCanvas = FindObjectOfType<Canvas>();
        StartCoroutine(StartCountdown());
        
    }

    IEnumerator StartCountdown()
    {
        countdownText.text = "5";
        yield return new WaitForSeconds(1f);
        countdownText.text = "4";
        yield return new WaitForSeconds(1f);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "Go!";
        countdownStarted = true;
        gameStarted = true;
        accuracy = 100;

        lineStartX = -675f;
        lineEndX = 675f;
        startTime = Time.time;
    }

    private void Update()
    {
        if (!countdownStarted || !gameStarted || gameEnded)
            return;

        if (hand == null)
            return;

        Vector3 handPosition = Leap.Unity.Utils.ToVector3(hand.PalmPosition);
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(handPosition);

        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            hostCanvas.transform as RectTransform,
            screenPosition,
            hostCanvas.worldCamera,
            out canvasPos
        );

        Debug.Log("On the world" + handPosition);
        Debug.Log("On the canvas" + canvasPos.x +"||||" + canvasPos.y);

        if (canvasPos.x >= lineStartX && canvasPos.x <= lineEndX)
        {

            float coveredDistance = Mathf.Clamp(
                canvasPos.x - lineStartX,
                0f,
                lineEndX - lineStartX
            );

            float distanceFromLine = Mathf.Abs(canvasPos.y - preGeneratedLine.position.y);
            float maxDistanceFromLine = 20f;
            accuracy = Mathf.Clamp01(1f - (distanceFromLine / maxDistanceFromLine)) * 100f;

            coveredDistanceText.text = $"Covered Distance: {coveredDistance}";
            scoreText.text = $"Accuracy: {accuracy}%";

            handTrail.Add(handPosition);
        }
        else if (canvasPos.x >= lineEndX)
        {
            gameEnded = true;
            duration = Time.time - startTime;

            DataTransfer.score_line = accuracy;
            DataTransfer.time_line = (float)Math.Round(duration*100)/100;


            Debug.Log("Game Over."+"Accuracy: "+ accuracy + "Time: "+duration);

            SaveHandTrailToJson();
        }
        else
        {
            coveredDistanceText.text = $"Covered Distance: 0";
            scoreText.text = $"Accuracy: 0%";
        }
    }

    private void OnEnable()
    {
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }

    private void OnDisable()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    void OnUpdateFrame(Frame frame)
    {
        hand = frame.Hands.Count > 0 ? frame.Hands[0] : null;
    }

    void SaveHandTrailToJson()
    {
        string json = JsonUtility.ToJson(new HandTrailData(handTrail));

        // File.WriteAllText(Application.persistentDataPath + "/HandTrail.json", json);

        string filePath = Application.dataPath + "/Resources/HandTrail.json";
        File.WriteAllText(filePath, json);
    }

    [System.Serializable]
    private class HandTrailData
    {
        public List<Vector3> trail;
        public float duration;

        public HandTrailData(List<Vector3> trail)
        {
            this.trail = trail;
            this.duration=duration;
        }
    }
}
