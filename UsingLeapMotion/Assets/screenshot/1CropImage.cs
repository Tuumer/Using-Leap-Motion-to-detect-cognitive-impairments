using System;
using System.IO;
using OpenCvSharp;

class Program
{
    static Mat ProcessTriangle(Mat img, int number)
    {
        int x1 = 690, y1 = 40;
        int x3 = 1690, y3 = 1040;
        int x2 = x1 + (x3 - x1) / 2, y2 = y1 + (y3 - y1) / 2;

        Point[][] triangles =
        {
            new[] {new Point(x2 + 134, y1), new Point(x2, y2), new Point(x3, y1)},
            new[] {new Point(x3, y1), new Point(x2, y2), new Point(x3, y2 - 134)},
            new[] {new Point(x3, y2 + 134), new Point(x2, y2), new Point(x3, y3)},
            new[] {new Point(x3, y3), new Point(x2, y2), new Point(x2 + 134, y3)},
            new[] {new Point(x2 - 134, y3), new Point(x2, y2), new Point(x1, y3)},
            new[] {new Point(x1, y3), new Point(x2, y2), new Point(x1, y2 + 134)},
            new[] {new Point(x1, y2 - 134), new Point(x2, y2), new Point(x1, y1)},
            new[] {new Point(x1, y1), new Point(x2, y2), new Point(x2 - 134, y1)}
        };

        Rect r = Cv2.BoundingRect(triangles[number]);

        Point[] triangleCropped =
        {
            new Point(triangles[number][0].X - r.X, triangles[number][0].Y - r.Y),
            new Point(triangles[number][1].X - r.X, triangles[number][1].Y - r.Y),
            new Point(triangles[number][2].X - r.X, triangles[number][2].Y - r.Y)
        };

        Console.WriteLine($"Triangle cropped coordinates: {string.Join(", ", triangleCropped)}");

        Mat imgCropped = new Mat(img, r);
        Mat warpMat = Cv2.GetAffineTransform(triangles[number], triangleCropped);
        Mat imgBackgroundCropped = new Mat();
        Cv2.WarpAffine(imgCropped, imgBackgroundCropped, warpMat, imgCropped.Size(), InterpolationFlags.Linear, BorderTypes.Reflect101);

        Mat mask = Mat.Zeros(r.Size(), MatType.CV_32FC3);
        Cv2.FillConvexPoly(mask, triangleCropped, Scalar.White, LineTypes.Link8, 0);

        Cv2.Multiply(imgBackgroundCropped, mask, imgBackgroundCropped);
        Cv2.Multiply(imgBackground.SubMat(r), 1 - mask, imgBackground.SubMat(r));
        Cv2.Add(imgBackground.SubMat(r), imgBackgroundCropped, imgBackground.SubMat(r));

        Mat imgRgb = new Mat();
        Cv2.CvtColor(imgBackground, imgRgb, ColorConversionCodes.BGR2RGB);

        int x1Crop = 15 + croppedRegions[number][0];
        int x2Crop = 15 + croppedRegions[number][1];
        int y1Crop = 80 + croppedRegions[number][2];
        int y2Crop = 80 + croppedRegions[number][3];
        Mat slicedImg = new Mat(imgRgb, new Rect(y1Crop, x1Crop, y2Crop - y1Crop, x2Crop - x1Crop));
        Mat resizedImg64 = new Mat();
        Cv2.Resize(slicedImg, resizedImg64, new Size(64, 64));

        return resizedImg64;
    }

    static void Main(string[] args)
    {
        string imagePath = "screenshot-2024-05-17-08-31-30.png";
        Mat img = Cv2.ImRead(imagePath, ImreadModes.Color);
        Cv2.BitwiseNot(img, img);

        string outputFolder = "cropped_images";
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        for (int number = 0; number < 8; number++)
        {
            Mat imgTri = ProcessTriangle(img, number);
            Cv2.ImWrite(Path.Combine(outputFolder, $"image_{number}.png"), imgTri);
        }
    }
}
using System;
using System.IO;
using OpenCvSharp;

class Program
{
    static Mat ProcessTriangle(Mat img, int number)
    {
        int x1 = 690, y1 = 40;
        int x3 = 1690, y3 = 1040;
        int x2 = x1 + (x3 - x1) / 2, y2 = y1 + (y3 - y1) / 2;

        Point[][] triangles =
        {
            new[] {new Point(x2 + 134, y1), new Point(x2, y2), new Point(x3, y1)},
            new[] {new Point(x3, y1), new Point(x2, y2), new Point(x3, y2 - 134)},
            new[] {new Point(x3, y2 + 134), new Point(x2, y2), new Point(x3, y3)},
            new[] {new Point(x3, y3), new Point(x2, y2), new Point(x2 + 134, y3)},
            new[] {new Point(x2 - 134, y3), new Point(x2, y2), new Point(x1, y3)},
            new[] {new Point(x1, y3), new Point(x2, y2), new Point(x1, y2 + 134)},
            new[] {new Point(x1, y2 - 134), new Point(x2, y2), new Point(x1, y1)},
            new[] {new Point(x1, y1), new Point(x2, y2), new Point(x2 - 134, y1)}
        };

        Rect r = Cv2.BoundingRect(triangles[number]);

        Point[] triangleCropped =
        {
            new Point(triangles[number][0].X - r.X, triangles[number][0].Y - r.Y),
            new Point(triangles[number][1].X - r.X, triangles[number][1].Y - r.Y),
            new Point(triangles[number][2].X - r.X, triangles[number][2].Y - r.Y)
        };

        Console.WriteLine($"Triangle cropped coordinates: {string.Join(", ", triangleCropped)}");

        Mat imgCropped = new Mat(img, r);
        Mat warpMat = Cv2.GetAffineTransform(triangles[number], triangleCropped);
        Mat imgBackgroundCropped = new Mat();
        Cv2.WarpAffine(imgCropped, imgBackgroundCropped, warpMat, imgCropped.Size(), InterpolationFlags.Linear, BorderTypes.Reflect101);

        Mat mask = Mat.Zeros(r.Size(), MatType.CV_32FC3);
        Cv2.FillConvexPoly(mask, triangleCropped, Scalar.White, LineTypes.Link8, 0);

        Cv2.Multiply(imgBackgroundCropped, mask, imgBackgroundCropped);
        Cv2.Multiply(imgBackground.SubMat(r), 1 - mask, imgBackground.SubMat(r));
        Cv2.Add(imgBackground.SubMat(r), imgBackgroundCropped, imgBackground.SubMat(r));

        Mat imgRgb = new Mat();
        Cv2.CvtColor(imgBackground, imgRgb, ColorConversionCodes.BGR2RGB);

        int x1Crop = 15 + croppedRegions[number][0];
        int x2Crop = 15 + croppedRegions[number][1];
        int y1Crop = 80 + croppedRegions[number][2];
        int y2Crop = 80 + croppedRegions[number][3];
        Mat slicedImg = new Mat(imgRgb, new Rect(y1Crop, x1Crop, y2Crop - y1Crop, x2Crop - x1Crop));
        Mat resizedImg64 = new Mat();
        Cv2.Resize(slicedImg, resizedImg64, new Size(64, 64));

        return resizedImg64;
    }

    static void Main(string[] args)
    {
        string imagePath = "screenshot-2024-05-17-08-31-30.png";
        Mat img = Cv2.ImRead(imagePath, ImreadModes.Color);
        Cv2.BitwiseNot(img, img);

        string outputFolder = "cropped_images";
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        for (int number = 0; number < 8; number++)
        {
            Mat imgTri = ProcessTriangle(img, number);
            Cv2.ImWrite(Path.Combine(outputFolder, $"image_{number}.png"), imgTri);
        }
    }
}
