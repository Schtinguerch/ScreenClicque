using System.Windows;

namespace ScreenClicque;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();

        RecognizerSettingsStackPanel.DataContext = new RecognizerSettingsModel(true);
        ImageProcessingSettingsStackPanel.DataContext = new ImageProcessingSettingsModel(true);

        SaveRecognizerSettingsButton.Click += (s, e) =>
        {
            var savedRecognizerSettings = (RecognizerSettingsModel) RecognizerSettingsStackPanel.DataContext;
            savedRecognizerSettings.Save<RecognizerSettingsModel>();

            CommonIsland.RecognizerSettings = savedRecognizerSettings;
        };

        SaveImageProcessingSettingsButton.Click += (s, e) =>
        {
            var savedImageProcessingSettings = (ImageProcessingSettingsModel) ImageProcessingSettingsStackPanel.DataContext;
            savedImageProcessingSettings.Save<RecognizerSettingsModel>();

            CommonIsland.ImageProcessingSettings = savedImageProcessingSettings;
        };
    }
}