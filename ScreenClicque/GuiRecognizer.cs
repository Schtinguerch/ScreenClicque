using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Point = System.Drawing.Point;

namespace ScreenClicque;

public class GuiRecognizer
{
    public List<Point> GetMinimumClickPoints()
    {
        var sortedPoints = GetClickPoints().OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
        return sortedPoints;
    }
    
    public List<Point> GetClickPoints(bool tryCompress = false, bool tryBlur = false)
    {
        var screenshot = GetScreenshot();
        var thresholdedImage = ProcessedImage(screenshot, tryCompress, tryBlur);
        var (contours, hierarchy) = FindContours(thresholdedImage);
        
        var points = new List<Point>();
        var lastPoint = new Point();

        for (var i = 0; i < contours.Size; i += 1)
        {
            var (isSuitable, bounds) = AnalyzeContour(contours[i], ref lastPoint);
            if (!isSuitable)
            {
                continue;
            }

            points.Add(lastPoint);
            CvInvoke.Rectangle(screenshot, bounds, new MCvScalar(0, 255, 0), 1);
        }

        CvInvoke.Imshow("test", screenshot);
        return points;
    }

    //Send "Hello" to the Uncle Bob
    private (bool, Rectangle) AnalyzeContour(VectorOfPoint contour, ref Point previousCenter)
    {
        var area = CvInvoke.ContourArea(contour);
        var notSuitableOutput = (false, new Rectangle(0, 0, 0, 0));

        if (area < 80 || area > 200_000)
        {
            return notSuitableOutput;
        }

        var rectangleBounds = CvInvoke.BoundingRectangle(contour);
        var widthRatio = (double) rectangleBounds.Width / rectangleBounds.Height;
        
        if (rectangleBounds.Height < 7 || rectangleBounds.Width < 7 || widthRatio > 18)
        {
            return notSuitableOutput;
        }

        var center = rectangleBounds.Center();
        if (center.Range(previousCenter) < 18)
        {
            return notSuitableOutput;
        }

        previousCenter = center;
        return (true, rectangleBounds);
    }

    private (VectorOfVectorOfPoint, Mat) FindContours(Image<Gray, byte> image)
    {
        var contours = new VectorOfVectorOfPoint();
        var hierarchy = new Mat();

        CvInvoke.FindContours(
            image,
            contours,
            hierarchy,
            RetrType.Tree,
            ChainApproxMethod.ChainApproxNone);

        return (contours, hierarchy);
    }

    private Image<Gray, byte> ProcessedImage(Image<Bgr, byte> sourceImage, bool tryCompress = false, bool tryBlur = false)
    {
        var grayscaledImage = sourceImage.Convert<Gray, byte>();

        if (tryCompress)
        {
            CvInvoke.Resize(grayscaledImage, grayscaledImage, new System.Drawing.Size(960, 540));
            CvInvoke.Resize(grayscaledImage, grayscaledImage, new System.Drawing.Size(1920, 1080));
        }

        if (tryBlur)
        {
            CvInvoke.GaussianBlur(grayscaledImage, grayscaledImage, new System.Drawing.Size(3, 3), 3, 3);
        }

        var thresholdedImage = grayscaledImage.ThresholdAdaptive(
            new Gray(255),
            AdaptiveThresholdType.MeanC,
            ThresholdType.Binary,
            5, new Gray(1));

        return thresholdedImage;
    }
    
    private Image<Bgr, byte> GetScreenshot()
    {
        using var bmp = new Bitmap(1920, 1080);
        using (var g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(0, 0, 0, 0, bmp.Size);
        }

        var image = bmp.ToImage<Bgr, byte>();
        return image;
    }
}