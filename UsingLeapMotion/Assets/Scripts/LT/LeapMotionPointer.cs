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


    private float sumX = 0f;
    private float sumY = 0f;
    private float sumZ = 0f;

    private float sumSquaredDiffX = 0f;
    private float sumSquaredDiffY = 0f;
    private float sumSquaredDiffZ = 0f;

    public static float meanX;
    public static float meanY;
    public static float meanZ;
    public static float squaredDiffX;
    public static float squaredDiffY;
    public static float squaredDiffZ;
    public static float sdX;
    public static float sdY;
    public static float sdZ;
    

    [SerializeField]
    private GameObject popup;

    private void Start()
    {
        hostCanvas = FindObjectOfType<Canvas>();
        Debug.Log(preGeneratedLine.position.y);
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

            float distanceFromLine = Mathf.Abs(canvasPos.y - (-105));
            float maxDistanceFromLine = 50f;
            accuracy = Mathf.Clamp01(1f - (distanceFromLine / maxDistanceFromLine)) * 100f;

            coveredDistanceText.text = $"Covered Distance: {coveredDistance}";
            scoreText.text = $"Accuracy: {accuracy}%";


            
            handTrail.Add(handPosition);


            sumX += handPosition.x;
            sumY += handPosition.y;
            sumZ += handPosition.z;

            int numPoints = handTrail.Count;

            meanX = sumX / numPoints;
            meanY = sumY / numPoints;
            meanZ = sumZ / numPoints;

            squaredDiffX = (handPosition.x - meanX) * (handPosition.x - meanX);
            squaredDiffY = (handPosition.y - meanY) * (handPosition.y - meanY);
            squaredDiffZ = (handPosition.z - meanZ) * (handPosition.z - meanZ);

            sumSquaredDiffX += squaredDiffX;
            sumSquaredDiffY += squaredDiffY;
            sumSquaredDiffZ += squaredDiffZ;

            sdX = Mathf.Sqrt(sumSquaredDiffX / numPoints);
            sdY = Mathf.Sqrt(sumSquaredDiffY / numPoints);
            sdZ = Mathf.Sqrt(sumSquaredDiffZ / numPoints);

            
        }
        else if (canvasPos.x >= lineEndX)
        {
            gameEnded = true;
            duration = Mathf.Round((Time.time-startTime) * 100f) / 100f;

            DataTransfer.score_line = (float)Math.Round(accuracy*100)/100;
            DataTransfer.time_line = duration;
            DataTransfer.state_line = true;

            if(popup!=null){
                popup.SetActive(true);
            }


            Debug.Log("Game Over."+"Accuracy: "+ accuracy + "Time: "+duration);

            Debug.Log($"Mean Hand Position: ({meanX}, {meanY}, {meanZ})");
            Debug.Log($"STD: ({sdX}, {sdY}, {sdZ})");

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

    public void checks()
    {
        DataTransfer.state_line = true;
    }

    void SaveHandTrailToJson()
    {
        string json = JsonUtility.ToJson(new HandTrailData(handTrail));

        DataTransfer.handoTrail = json;

        // File.WriteAllText(Application.persistentDataPath + "/HandTrail.json", json);

        string filePath = Application.dataPath + "/Resources/HandTrail.json";
        File.WriteAllText(filePath, json);
    }

    [System.Serializable]
    private class HandTrailData
    {
        public List<Vector3> trail;
        

        public HandTrailData(List<Vector3> trail)
        {
            this.trail = trail;
            
        }
    }
}
