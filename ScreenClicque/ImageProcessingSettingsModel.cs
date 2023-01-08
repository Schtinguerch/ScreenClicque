using System;
using Newtonsoft.Json;

namespace ScreenClicque;

public class ImageProcessingSettingsModel : LoadableModel
{
    private int _thresholdBlockSize = 5;

    public int ThresholdBlockSize
    {
        get => _thresholdBlockSize;
        set
        {
            if (value < 0 || value % 2 != 1)
            {
                throw new ArgumentException("Block size should be odd and > 0");
            }
        }
    }
    
    public ObservableXY BlurKernelSize { get; set; } = new ObservableXY(3, 3);
    public ObservableXY BlurSigma { get; set; } = new ObservableXY(3, 3);
    public ObservableXY CompressedImageSize { get; set; } = new ObservableXY(960, 540);
    
    public override string FilePath { get; } = "ImageSettings.json";
    public override void PullFrom(LoadableModel? model)
    {
        var settings = (ImageProcessingSettingsModel) model;

        ThresholdBlockSize = settings.ThresholdBlockSize;
        BlurKernelSize = settings.BlurKernelSize;
        BlurSigma = settings.BlurSigma;
        CompressedImageSize = settings.CompressedImageSize;
    }
    
    public ImageProcessingSettingsModel() {}
    public ImageProcessingSettingsModel(bool loadFromDefaultFile)
    {
        if (loadFromDefaultFile)
        {
            Load<ImageProcessingSettingsModel>();
        }
    }
}