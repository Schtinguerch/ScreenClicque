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