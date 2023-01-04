using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Point = System.Drawing.Point;

namespace ScreenClicque;

public class LetterPlacer
{
    public readonly Window TargetWindow;

    private List<Point> _locations;
    private List<LetterControl> _letters;
    
    private string _pressedShortCut;
    private int _maxShortcutLength = 2;

    private string _rawLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string _specialLetters = "1234567890;/\"";

    
}