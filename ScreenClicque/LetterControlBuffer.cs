using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace ScreenClicque;

public class LetterControlBuffer
{
    public readonly List<LetterControl> Controls = new List<LetterControl>();

    public List<LetterControl> CompareControlCount(int requiredCount)
    {
        var delta = requiredCount - Controls.Count;
        return delta <= 0 ? new List<LetterControl>() : AllocateControls(delta);
    }

    public LetterControlBuffer(int startCount = 200)
    {
        AllocateControls(startCount);
    }

    public void HideAllControls()
    {
        foreach (var control in Controls)
        {
            control.Visibility = Visibility.Hidden;
        }
    }

    public void ShowControls(int count = 0)
    {
        if (count <= 0)
        {
            count = Controls.Count;
        }

        for (int i = 0; i < count; i += 1)
        {
            Controls[i].Visibility = Visibility.Visible;
        }
    }

    private List<LetterControl> AllocateControls(int count)
    {
        if (count < 0)
        {
            Debug.WriteLine("No controls allocation");
            return new List<LetterControl>();
        }

        var allocatedControls = Enumerable.Range(0, count).Select(x => new LetterControl()).ToList();
        Controls.AddRange(allocatedControls);
        return allocatedControls;
    }
}