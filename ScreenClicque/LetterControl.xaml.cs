using System.Windows.Controls;

namespace ScreenClicque;

public partial class LetterControl : UserControl
{
    public string LettersCharacter
    {
        get => LetterTextBlock.Text;
        set => LetterTextBlock.Text = value;
    }
    
    public LetterControl()
    {
        InitializeComponent();
    }
}