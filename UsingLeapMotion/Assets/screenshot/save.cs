using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageSplitterwww : MonoBehaviour
{
    public Texture2D originalImage;
    public Button splitButton;

    void Start()
    {
        if (originalImage == null)
        {
            UnityEngine.Debug.LogError("Original image not assigned!");
            return;
        }

        if (splitButton != null)
        {
            splitButton.onClick.AddListener(SplitImage);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Split button not assigned!");
        }
    }

    void SplitImage()
    {
        // Split the image into two halves
        int width = originalImage.width;
        int height = originalImage.height;

        Rect leftRect = new Rect(0, 0, width / 2, height);
        Rect rightRect = new Rect(width / 2, 0, width / 2, height);

        Texture2D leftHalf = new Texture2D((int)leftRect.width, (int)leftRect.height);
        Texture2D rightHalf = new Texture2D((int)rightRect.width, (int)rightRect.height);

        leftHalf.SetPixels(originalImage.GetPixels((int)leftRect.x, (int)leftRect.y, (int)leftRect.width, (int)leftRect.height));
        rightHalf.SetPixels(originalImage.GetPixels((int)rightRect.x, (int)rightRect.y, (int)rightRect.width, (int)rightRect.height));

        leftHalf.Apply();
        rightHalf.Apply();

        // Save the halves as image files in the "cropped_images" folder
        string folderPath = UnityEngine.Application.dataPath + "/screenshot/cropped_images";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        byte[] leftBytes = leftHalf.EncodeToPNG();
        byte[] rightBytes = rightHalf.EncodeToPNG();

        File.WriteAllBytes(Path.Combine(folderPath, "LeftHalf.png"), leftBytes);
        File.WriteAllBytes(Path.Combine(folderPath, "RightHalf.png"), rightBytes);

        UnityEngine.Debug.Log("Images saved successfully in the 'cropped_images' folder.");
    }
}
