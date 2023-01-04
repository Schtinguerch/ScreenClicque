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