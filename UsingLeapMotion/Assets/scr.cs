using UnityEngine;
using System.Collections.Generic;

public class SpriteCreator : MonoBehaviour
{
    public GameObject spritePrefab;
    public Texture2D circleTexture;
    public List<string> initialBellTags = new List<string>() { 
    "bell1", "bell2", "bell3", "bell4", "bell5", "bell6", "bell7", "bell8", "bell9", "bell10",
    "bell11", "bell12", "bell13", "bell14", "bell15", "bell16", "bell17", "bell18", "bell19", "bell20",
    "bell21", "bell22", "bell23", "bell24", "bell25", "bell26", "bell27", "bell28", "bell29", "bell30",
    "bell31", "bell32", "bell33", "bell34", "bell35"
    };


    private HashSet<string> clickedBellTags = new HashSet<string>(); // Changed data structure to HashSet

    public int count = 0;

    // Define a LayerMask variable to specify which layers are selectable
    public LayerMask selectableLayerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider != null && IsSelectable(hit.collider.gameObject))
                {
                    string tag = hit.collider.gameObject.tag;
                    clickedBellTags.Add(tag);
                }
            }
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Instantiate(spritePrefab, worldPosition, Quaternion.identity);
            DrawCircle(worldPosition, 0.05f);
            count++;
        }

        // Compare the clicked bell tags with the initial list
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CompareTags();
        }
    }

    bool IsSelectable(GameObject obj)
    {
        // Check if the GameObject is on the selectable layer mask
        bool selectable = (selectableLayerMask & (1 << obj.layer)) != 0;
        if (!selectable)
        {
            Debug.Log(obj.name + " is not selectable because it is not on a selectable layer.");
        }
        return selectable;
    }

    void DrawCircle(Vector3 position, float radius)
    {
        GameObject circle = new GameObject("Circle");
        circle.transform.position = position;

        SpriteRenderer renderer = circle.AddComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(circleTexture, new Rect(0, 0, circleTexture.width, circleTexture.height), new Vector2(0.5f, 0.5f));

        float diameter = radius * 2f;
        circle.transform.localScale = new Vector3(diameter, diameter, 1f);
    }

    void CompareTags()
    {
        Debug.Log("Clicked Bell Tags:");
        foreach (string tag in clickedBellTags)
        {
            Debug.Log(tag);
        }

        // Compare clicked bell tags with initial list
        bool allBellsClicked = true;
        foreach (string bellTag in initialBellTags)
        {
            if (!clickedBellTags.Contains(bellTag))
            {
                allBellsClicked = false;
                break;
            }
        }

        if (allBellsClicked)
        {
            Debug.Log("All bells clicked!");
            Debug.Log(clickedBellTags.Count + " / " + count);
        }
        else
        {
            Debug.Log("Not all bells clicked.");
            Debug.Log(clickedBellTags.Count + " / " + count);
        }
    }
}
