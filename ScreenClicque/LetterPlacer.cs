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

    public LetterPlacer(Window window)
    {
        TargetWindow = window;
        window.KeyDown += WindowOnKeyDown;
    }

    public void StartSelection(List<Point> locations, List<LetterControl> letters)
    {
        _locations = locations;
        _letters = letters;
        _pressedShortCut = string.Empty;

        DeclareCombinations();

        for (var i = 0; i < locations.Count; i += 1)
        {
            Canvas.SetLeft(letters[i], locations[i].X);
            Canvas.SetTop(letters[i], locations[i].Y);
        }
        
        TargetWindow.Show();
        TargetWindow.Activate();
    }

    private void DeclareCombinations()
    {
        _maxShortcutLength = 2;
        var count = 0;
        var isLongList = _locations.Count > _rawLetters.Length * _rawLetters.Length;

        for (var i = 0; i < _rawLetters.Length; i += 1)
        for (var j = 0; j < _rawLetters.Length; j += 1)
        {
            if (count >= _locations.Count)
            {
                //I know, we all hate the goto, but I'm lazy and know what I do
                goto lettersMarked;
            }

            if (isLongList && (i is > 8 and < 14))
            {
                continue;
            }

            count += 1;
            _letters[count - 1].LettersCharacter = $"{_rawLetters[i]}{_rawLetters[j]}";
        }
        

        if (count < _locations.Count)
        {
            _maxShortcutLength = 3;

            foreach (var letter in "JKLMN")
            {
                for (var j = 0; j < _rawLetters.Length; j += 1)
                for (var k = 0; k < _rawLetters.Length; k += 1)
                {
                    if (count >= _locations.Count)
                    {
                        goto lettersMarked;
                    }
                
                    count += 1;
                    _letters[count - 1].LettersCharacter = $"{letter}{_rawLetters[j]}{_rawLetters[k]}";
                }
            }
        }

        lettersMarked:
        var taskbarCount = 0;

        for (var i = 0; i < _locations.Count; i += 1)
        {
            if (taskbarCount >= _specialLetters.Length)
            {
                break;
            }

            if ((_locations[i].Y > 1030) && (_locations[i].X is > 600 and < 1400))
            {
                taskbarCount += 1;
                _letters[i].LettersCharacter = $"{_specialLetters[taskbarCount - 1]}";
            }
        }
    }

    private void WindowOnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            return;
        }

        var pressedKey = e.Key.ToString();
        if (pressedKey.Length > 1)
        {
            pressedKey = pressedKey
                .Replace("D", "")
                .Replace("Oem1", ";")
                .Replace("Oem2", "/")
                .Replace("Oem7", "\"");
        }
        
        _pressedShortCut += pressedKey;
        

        var hotKeyIndex = _letters.FindIndex(l => l.LettersCharacter == _pressedShortCut);
        if (hotKeyIndex == -1)
        {
            if (_pressedShortCut.Length > _maxShortcutLength)
            {
                TargetWindow.Hide();
            }
            
            return;
        }

        TargetWindow.Hide();
        Mouser.ClickAt(_locations[hotKeyIndex]);
    }
}