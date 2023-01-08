using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace ScreenClicque;

public class GuiRecognizer
{
    private Size _screenSize;
    
    public List<Point> GetMinimumClickPoints()
    {
        var sortedPoints = GetClickPoints().OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
        return sortedPoints;
    }
    
    public List<Point> GetClickPoints()
    {
        var screenshot = GetScreenshot();
        var thresholdedImage = ProcessedImage(screenshot);
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

        if (CommonIsland.RecognizerSettings.OpenOpenCvWindow)
        {
            CvInvoke.Imshow("OpenCV recognition debug", screenshot);
        }
        
        return points;
    }

    //Send "Hello" to the Uncle Bob
    private (bool, Rectangle) AnalyzeContour(VectorOfPoint contour, ref Point previousCenter)
    {
        var area = CvInvoke.ContourArea(contour);
        var notSuitableOutput = (false, new Rectangle(0, 0, 0, 0));

        if (area < CommonIsland.RecognizerSettings.MinShapeArea || area > CommonIsland.RecognizerSettings.MaxShapeArea)
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

    private Image<Gray, byte> ProcessedImage(Image<Bgr, byte> sourceImage)
    {
        var grayscaledImage = sourceImage.Convert<Gray, byte>();

        if (CommonIsland.RecognizerSettings.UseImageCompression)
        {
            var compressedSize = CommonIsland.ImageProcessingSettings.CompressedImageSize.ToSize();
            
            CvInvoke.Resize(grayscaledImage, grayscaledImage, compressedSize);
            CvInvoke.Resize(grayscaledImage, grayscaledImage, _screenSize);
        }

        if (CommonIsland.RecognizerSettings.UseBlur)
        {
            CvInvoke.GaussianBlur(
                grayscaledImage, 
                grayscaledImage, 
                CommonIsland.ImageProcessingSettings.BlurKernelSize.ToSize(), 
                CommonIsland.ImageProcessingSettings.BlurSigma.X, 
                CommonIsland.ImageProcessingSettings.BlurSigma.Y);
        }

        var thresholdedImage = grayscaledImage.ThresholdAdaptive(
            new Gray(255),
            AdaptiveThresholdType.MeanC,
            ThresholdType.Binary,
            CommonIsland.ImageProcessingSettings.ThresholdBlockSize, 
            new Gray(1));

        return thresholdedImage;
    }
    
    private Image<Bgr, byte> GetScreenshot()
    {
        UpdateScreenSize();
        using var bmp = new Bitmap(_screenSize.Width, _screenSize.Height);
        
        using (var g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(0, 0, 0, 0, bmp.Size);
        }

        var image = bmp.ToImage<Bgr, byte>();
        return image;
    }

    private void UpdateScreenSize() => _screenSize = new Size(
        (int) SystemParameters.PrimaryScreenWidth,
        (int) SystemParameters.PrimaryScreenHeight);
}