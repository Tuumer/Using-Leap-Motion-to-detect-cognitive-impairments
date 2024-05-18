using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static Bitmap ProcessTriangle(Bitmap img, int number)
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

        Point[] triangle = triangles[number];
        Rectangle r = new Rectangle(triangle[0].X, triangle[0].Y, triangle[2].X - triangle[0].X, triangle[2].Y - triangle[0].Y);

        Bitmap imgCropped = img.Clone(r, img.PixelFormat);
        Bitmap imgBackground = new Bitmap(img.Width, img.Height);

        using (Graphics g = Graphics.FromImage(imgBackground))
        {
            g.Clear(Color.White);
            g.DrawImage(imgCropped, new Rectangle(r.X, r.Y, r.Width, r.Height));
        }

        Bitmap imgRgb = new Bitmap(imgBackground.Width, imgBackground.Height);
        using (Graphics g = Graphics.FromImage(imgRgb))
        {
            g.Clear(Color.White);
            g.DrawImage(imgBackground, new Rectangle(0, 0, imgBackground.Width, imgBackground.Height));
        }

        int[][] croppedRegions =
        {
            new[] {y1 + 15, y1 + 15 + 270, x2 + 80, x2 + 80 + 270},
            new[] {y2 - 80 - 270, y2 - 80, x3 - 15 - 270, x3 - 15},
            new[] {y2 + 80, y2 + 80 + 270, x3 - 15 - 270, x3 - 15},
            new[] {y3 - 15 - 270, y3 - 15, x2 + 80, x2 + 80 + 270},
            new[] {y3 - 15 - 270, y3 - 15, x2 - 80 - 270, x2 - 80},
            new[] {y2 + 80, y2 + 80 + 270, x1 + 15, x1 + 15 + 270},
            new[] {y2 - 80 - 270, y2 - 80, x1 + 15, x1 + 15 + 270},
            new[] {y1 + 15, y1 + 15 + 270, x2 - 80 - 270, x2 - 80}
        };

        int x1Crop = croppedRegions[number][0];
        int x2Crop = croppedRegions[number][1];
        int y1Crop = croppedRegions[number][2];
        int y2Crop = croppedRegions[number][3];
        Rectangle cropRect = new Rectangle(y1Crop, x1Crop, y2Crop - y1Crop, x2Crop - x1Crop);
        Bitmap slicedImg = imgRgb.Clone(cropRect, imgRgb.PixelFormat);
        Bitmap resizedImg64 = new Bitmap(slicedImg, new Size(64, 64));

        return resizedImg64;
    }

    static void Main(string[] args)
    {
        string imagePath = "screenshot-2024-05-17-08-31-30.png";
        Bitmap img = new Bitmap(imagePath);

        using (Graphics g = Graphics.FromImage(img))
        {
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
        }

        string outputFolder = "cropped_images";
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        for (int number = 0; number < 8; number++)
        {
            Bitmap imgTri = ProcessTriangle(img, number);
            imgTri.Save(Path.Combine(outputFolder, $"image_{number}.png"), ImageFormat.Png);
        }
    }
}
