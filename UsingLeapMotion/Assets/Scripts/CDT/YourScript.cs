using UnityEngine;

public class YourScript : MonoBehaviour
{
    // Reference to the GameObject with the UpdateText script attached
    public GameObject textObject;

    void Start()
    {
        // Call main() function from Python script
        int trueCount = new_python_script.main();

        // Get the UpdateText component and call UpdateCount function
        UpdateText updateText = textObject.GetComponent<UpdateText>();
        updateText.UpdateCount(trueCount);
    }
}
