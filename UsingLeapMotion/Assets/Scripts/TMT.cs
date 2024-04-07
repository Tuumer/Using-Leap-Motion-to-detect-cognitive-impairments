using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT : MonoBehaviour
{
    [SerializeField] private GameObject circle;
    [SerializeField] private int numCircles;
    private int[] numberOrder;
    private List<GameObject> circles;

    // Start is called before the first frame update
    void Start()
    {
        circles = new List<GameObject>();
        SetupData();
        PlaceCircles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupData(){
        numberOrder = new int[numCircles];
        for(int i=0;i<numCircles;i++){
            numberOrder[i]=i+1;
        }
    }

    private void PlaceCircles(){
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        float padding=50f;
        float radius=30f;

        for(int i=0;i<numCircles;i++){
            float randomX = UnityEngine.Random.Range(padding+radius,screenWidth-padding-radius);
            float randomY = UnityEngine.Random.Range(padding+radius,screenHeight-padding-radius);
            GameObject circleObject = Instantiate(circle,new Vector3(randomX,randomY,0f),Quaternion.identity);

            Text textComp = circleObject.GetComponentInChildren<Text>();
            if (textComp != null){
                textComp.text = (numberOrder != null) ? numberOrder[i].ToString() : letterNumberOrder[i];
                }
            circles.Add(circleObject);
        }
    }
}
