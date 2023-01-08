using System.Drawing;

namespace ScreenClicque;

public class ObservableXY : ObservableModel
{
    private int _x, _y;

    public int X
    {
        get => _x;
        set
        {
            _x = value;
            OnPropertyChanged(nameof(X));
        }
    }

    public int Y
    {
        get => _y;
        set
        {
            _y = value;
            OnPropertyChanged(nameof(X));
        }
    }

    public ObservableXY() {}
    public ObservableXY(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Size ToSize() => new Size(X, Y);
}