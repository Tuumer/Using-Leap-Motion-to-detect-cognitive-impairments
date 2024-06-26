using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Debug = UnityEngine.Debug;

public class ImageProcessor : MonoBehaviour
{
    public Button processButton; // Assign your process button in the Unity Inspector

    private int imageCounter = 1; // Counter for naming images

    public void ProcessImage()
    {
        // Find the original image in the Assets folder
        string[] imagePaths = Directory.GetFiles(Application.dataPath, "aaa.png", SearchOption.AllDirectories);

        if (imagePaths.Length == 0)
        {
            Debug.LogError("No PNG images found in the Assets folder!");
            return;
        }

        // Load the first PNG image found
        string imagePath = imagePaths[0].Replace(Application.dataPath, "Assets");
        Texture2D originalImage = LoadImageFromFile(imagePath);

        if (originalImage == null)
        {
            Debug.LogError("Failed to load the original image!");
            return;
        }

        Debug.Log($"Original image loaded from: {imagePath}");

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
                    int srcX = (int)region.x + x;
                    int srcY = (int)region.y + y;
                    int index = srcY * originalImage.width + srcX;

                    // Check if index is within bounds
                    if (srcX < originalImage.width && srcY < originalImage.height)
                    {
                        croppedTexture.SetPixel(x, y, pixels[index]);
                    }
                    else
                    {
                        Debug.LogError($"Index out of bounds: srcX={srcX}, srcY={srcY}, index={index}");
                    }
                }
            }
            croppedTexture.Apply();

            // Resize the cropped texture to 64x64
            Texture2D resizedCroppedTexture = ResizeTexture(croppedTexture, 64, 64);

            // Save the resized cropped texture as PNG with sequential name
            byte[] bytes = resizedCroppedTexture.EncodeToPNG();
            string folderPath = Path.Combine(Application.dataPath, "screenshots/cropped_images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllBytes(Path.Combine(folderPath, $"img{imageCounter}.png"), bytes);
            imageCounter++; // Increment image counter

            // Clean up
            Destroy(croppedTexture);
            Destroy(resizedCroppedTexture);
        }

        Debug.Log("Images saved successfully in the 'cropped_images' folder.");
        DataTransfer.state_cdt = true;
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

    // Load an image from file
    private Texture2D LoadImageFromFile(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData))
        {
            return texture;
        }
        return null;
    }
}
