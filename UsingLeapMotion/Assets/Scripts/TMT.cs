using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TMT : MonoBehaviour
{
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private int numCircles;
    private int[] numberOrder;
    [SerializeField]private List<GameObject> circles;
    [SerializeField] TextMeshProUGUI textComp;


    // Start is called before the first frame update
    public void Start()
    {
        circles = new List<GameObject>();
        SetupData();
        PlaceCircles();
    }

    private void SetupData(){
        numberOrder = new int[numCircles];
        for(int i=0;i<numCircles;i++){
            numberOrder[i]=i+1;
            Debug.Log(numberOrder[i]);
        }
    }

    private void PlaceCircles(){

        RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
        
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        float padding=100f;
        float radius=100f;

        for(int i=0;i<numCircles;i++){
            float randomX = UnityEngine.Random.Range(padding+radius,screenWidth-padding-radius);
            float randomY = UnityEngine.Random.Range(padding+radius,screenHeight-padding-radius);
            
            GameObject circleObject = Instantiate(circlePrefab,new Vector3(randomX,randomY,0f),Quaternion.identity);
            

            if(textComp !=null){
                textComp.text=(numberOrder[i]+1).ToString();
                Debug.Log("hello");
            }

            circleObject.transform.SetParent(transform);
            circles.Add(circleObject);
        }
    }
}

// public class TMT : MonoBehaviour
// {
//     [SerializeField] private GameObject circlePrefab;
//     [SerializeField] private int numCircles;
//     private int[] numberOrder;
//     [SerializeField]private List<GameObject> circles;
//     [SerializeField] TextMeshProUGUI textComp;


//     // Start is called before the first frame update
//     public void Start()
//     {
//         circles = new List<GameObject>();
//         SetupData();
//         PlaceCircles();
//     }

//     private void SetupData(){
//         numberOrder = new int[numCircles];
//         for(int i = 0; i < numCircles; i++){
//             numberOrder[i] = i + 1;
//             Debug.Log(numberOrder[i]);
//         }
//     }

//     private void PlaceCircles(){

//         RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
        
//         float screenWidth = Screen.width;
//         float screenHeight = Screen.height;
        
//         float padding = 100f;
//         float radius = 100f;

//         for(int i = 0; i < numCircles; i++){
//             bool overlapping = true;
//             float randomX = 0f;
//             float randomY = 0f;

//             // Keep generating random positions until a non-overlapping position is found
//             while (overlapping) {
//                 randomX = UnityEngine.Random.Range(padding + radius, screenWidth - padding - radius);
//                 randomY = UnityEngine.Random.Range(padding + radius, screenHeight - padding - radius);
//                 overlapping = CheckOverlap(new Vector2(randomX, randomY), radius);
//             }
            
//             GameObject circleObject = Instantiate(circlePrefab, new Vector3(randomX, randomY, 0f), Quaternion.identity);
            
//             if(textComp != null){
//                 textComp.text = (numberOrder[i] + 1).ToString();
//                 Debug.Log("hello");
//             }

//             circleObject.transform.SetParent(transform);
//             circles.Add(circleObject);
//         }
//     }

//     private bool CheckOverlap(Vector2 newPosition, float radius) {
//         foreach (GameObject circle in circles) {
//             if (Vector2.Distance(circle.transform.position, newPosition) < radius * 2) {
//                 // Circles overlap
//                 return true;
//             }
//         }
//         // No overlap found
//         return false;
//     }
// }

