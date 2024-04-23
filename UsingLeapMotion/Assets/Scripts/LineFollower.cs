using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

// public class LineFollower : MonoBehaviour
// {
//     [SerializeField]public float speed;
//     private float score;
   
//     // const float maxDistance = Mathf.Max(1.0f, 1.0f, 1.0f);
//     // const float minPoints = 0f;
//     // const float pointsPerUnit = 10f;

//     void Start(){
        
//     }
//     void Update()
//     {
//     RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
//     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//     mousePos.z = 0; // Set z-position to 0 for 2D movement

//     // Adjust position based on canvas position and size
//     Vector3 canvasCenter = canvasRect.position;
//     Vector2 canvasSize = canvasRect.rect.size;
//     Vector3 lineCenter = new Vector3(1750f / 2f, 0f, 0f);

//     mousePos.x = canvasCenter.x + mousePos.x * canvasSize.x - canvasRect.anchoredPosition.x;
//     mousePos.y = canvasCenter.y + mousePos.y * canvasSize.y - canvasRect.anchoredPosition.y;

//     transform.localPosition = Vector3.Lerp(transform.localPosition, mousePos, speed * Time.deltaTime);

//     float distanceToCenter = Vector3.Distance(transform.localPosition, lineCenter);

//     // score = Mathf.Clamp(minPoints + (maxDistance - distanceToCenter) * pointsPerUnit, minPoints, maxDistance * pointsPerUnit);

//     Debug.Log(distanceToCenter);
//   }
// }


public class LineFollower : MonoBehaviour
{
    [SerializeField] public float speed;
    private float score;
    [SerializeField] private List<Vector3> trail = new List<Vector3>();

    void Start()
    {
        StartCoroutine(UpdateWithInterval(0.5f));
    }

    IEnumerator UpdateWithInterval(float interval)
    {
        while (true)
        {
            // Your existing Update() logic here
            // RectTransform canvasRect = transform.parent.GetComponent<RectTransform>().position;
            // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // transform.position = new Vector3(mousePos.x,mousePos.y,mousePos.z);


            // // Adjust position based on canvas position and size
            // Vector3 canvasCenter = canvasRect.position;
            // Vector2 canvasSize = canvasRect.rect.size;
            // Vector3 lineCenter = new Vector3(1536f / 2f, 0f, 0f);

            // mousePos.x = canvasCenter.x + mousePos.x * canvasSize.x - canvasRect.anchoredPosition.x;
            // mousePos.y = canvasCenter.y + mousePos.y * canvasSize.y - canvasRect.anchoredPosition.y;

            // transform.localPosition = Vector3.Lerp(transform.localPosition, mousePos, speed * Time.deltaTime);

            // float distanceToCenter = Vector3.Distance(transform.localPosition, lineCenter);

            // score = Mathf.Clamp(minPoints + (maxDistance - distanceToCenter) * pointsPerUnit, minPoints, maxDistance * pointsPerUnit);

            Debug.Log("Current pos: " + transform.localPosition);
            trail.Add(transform.localPosition);

            yield return new WaitForSeconds(interval);
        }
    }

    public void checkArray()
    {
        foreach (Vector3 tr in trail)
        {
            Debug.Log("Mouse position: " + tr);
        }
    }


}