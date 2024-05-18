using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Debug = UnityEngine.Debug; // Alias for UnityEngine.Debug

public class ImageProcessor : MonoBehaviour
{
    public Texture2D originalImage; // Assign your original image in the Unity Inspector
    public Button processButton; // Assign your process button in the Unity Inspector

    private int imageCounter = 1; // Counter for naming images

    void Start()
    {
        if (originalImage == null)
        {
            Debug.LogError("Original image not assigned!");
            return;
        }

        if (processButton != null)
        {
            processButton.onClick.AddListener(ProcessImage);
        }
        else
        {
            Debug.LogWarning("Process button not assigned!");
        }
    }

    void ProcessImage()
    {
        // Resize the original image to 1920x1080
        originalImage = ResizeTexture(originalImage, 1920, 1080);

        Debug.Log($"Original image dimensions after resize: {originalImage.width} x {originalImage.height}");

        // Define cropping coordinates
        int x1 = 690, y1 = 40;
        int x3 = 1690, y3 = 1040;
        int x2 = x1 + (x3 - x1) / 2, y2 = y1 + (y3 - y1) / 2;

        // Define cropping regions
        Rect[] croppedRegions = new Rect[]
        {
            new Rect(x2 + 80, y1 + 15, 270, 270),
            new Rect(x3 - 15 - 270, y2 - 80 - 270, 270, 270),
            new Rect(x3 - 15 - 270, y2 + 80, 270, 270),
            new Rect(x2 + 80, y3 - 15 - 270, 270, 270),
            new Rect(x2 - 80 - 270, y3 - 15 - 270, 270, 270),
            new Rect(x1 + 15, y2 + 80, 270, 270),
            new Rect(x1 + 15, y2 - 80 - 270, 270, 270),
            new Rect(x2 - 80 - 270, y1 + 15, 270, 270)
        };

        // Convert Texture2D to Color32 array
        Color32[] pixels = originalImage.GetPixels32();

        // Process each cropped region
        foreach (Rect region in croppedRegions)
        {
            Debug.Log($"Cropping region: X={region.x}, Y={region.y}, Width={region.width}, Height={region.height}");

            // Create a new Texture2D for the cropped region
            Texture2D croppedTexture = new Texture2D((int)region.width, (int)region.height);

            // Copy pixels for the cropped region
            for (int y = 0; y < region.height; y++)
            {
                for (int x = 0; x < region.width; x++)
                {
                    int index = (int)(region.y + y) * originalImage.width + (int)(region.x + x);

                    // Check if index is within bounds
                    if (index >= 0 && index < pixels.Length)
                    {
                        croppedTexture.SetPixel(x, y, pixels[index]);
                    }
                    else
                    {
                        Debug.LogError($"Index out of bounds: {index}");
                    }
                }
            }
            croppedTexture.Apply();

            // Save the cropped texture as PNG with sequential name
            byte[] bytes = croppedTexture.EncodeToPNG();
            string folderPath = Application.dataPath + "/screenshots/cropped_images";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllBytes(Path.Combine(folderPath, $"img{imageCounter}.png"), bytes);
            imageCounter++; // Increment image counter

            // Clean up
            Destroy(croppedTexture);
        }

        Debug.Log("Images saved successfully in the 'cropped_images' folder.");
    }

    // Resize a Texture2D to the specified dimensions
    private Texture2D ResizeTexture(Texture2D texture, int width, int height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(texture, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D resizedTexture = new Texture2D(width, height);
        resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        resizedTexture.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);
        return resizedTexture;
    }
}
