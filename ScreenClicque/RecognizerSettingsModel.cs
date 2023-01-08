using System;
using System.IO;
using Newtonsoft.Json;

namespace ScreenClicque;

public class RecognizerSettingsModel : LoadableModel
{
    private int _minShapeArea = 80;
    private int _maxShapeArea = 200_000;

    private bool _useBlur = false;
    private bool _useImageCompression = false;
    private bool _openOpenCvWindow = false;

    public int MinShapeArea
    {
        get => _minShapeArea;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Min area cannot be < 0");
            }

            _minShapeArea = value;
            OnPropertyChanged(nameof(MinShapeArea));
        }
    }

    public int MaxShapeArea
    {
        get => _maxShapeArea;
        set
        {
            if (value < 0 || value < _minShapeArea)
            {
                throw new ArgumentException("Min area cannot be < 0 or < min area");
            }

            _maxShapeArea = value;
            OnPropertyChanged(nameof(MaxShapeArea));
        }
    }

    public bool UseBlur
    {
        get => _useBlur;
        set
        {
            _useBlur = value;
            OnPropertyChanged(nameof(UseBlur));
        }
    }

    public bool UseImageCompression
    {
        get => _useImageCompression;
        set
        {
            _useImageCompression = value;
            OnPropertyChanged(nameof(UseImageCompression));
        }
    }
    
    public bool OpenOpenCvWindow
    {
        get => _openOpenCvWindow;
        set
        {
            _openOpenCvWindow = value;
            OnPropertyChanged(nameof(OpenOpenCvWindow));
        }
    }

    public RecognizerSettingsModel() {}
    public RecognizerSettingsModel(bool loadFromDefaultFile)
    {
        if (loadFromDefaultFile)
        {
            Load<RecognizerSettingsModel>();
        }
    }

    public override string FilePath { get; } = "RecognizerSettings.json";
    public override void PullFrom(LoadableModel? model)
    {
        var settings = (RecognizerSettingsModel) model;
        
        MinShapeArea = settings.MinShapeArea;
        MaxShapeArea = settings.MaxShapeArea;
        
        UseBlur = settings.UseBlur;
        UseImageCompression = settings.UseImageCompression;
        OpenOpenCvWindow = settings.OpenOpenCvWindow;
    }
}