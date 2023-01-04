using System.Collections.Generic;

namespace ScreenClicque;

public class HotkeyController
{
    public GuiRecognizer Recognizer { get; set; }
    public LetterControlBuffer Buffer { get; set; }
    public ClicqueMenu GuiClickMenu { get; set; }
    public LetterPlacer Placer { get; set; }

    public HotkeyController(ClicqueMenu clicqueMenuWindow)
    {
        GuiClickMenu = clicqueMenuWindow;
        GuiClickMenu.BackgroundClick += OnHookedBackgroundClick;

        Buffer = new LetterControlBuffer();
        AddNewLetters(Buffer.Controls);
        Buffer.HideAllControls();
        
        Recognizer = new GuiRecognizer();
        Placer = new LetterPlacer(GuiClickMenu);
    }

    private void OnHookedBackgroundClick()
    {
        var locations = Recognizer.GetMinimumClickPoints();
        var newLetters = Buffer.CompareControlCount(locations.Count);
        
        Buffer.HideAllControls();
        Buffer.ShowControls(locations.Count);

        AddNewLetters(newLetters);
        Placer.StartSelection(locations, Buffer.Controls);
    }

    private void AddNewLetters(List<LetterControl> letters)
    {
        foreach (var letter in letters)
        {
            GuiClickMenu.ClicquesCanvas.Children.Add(letter);
        }
    }
}