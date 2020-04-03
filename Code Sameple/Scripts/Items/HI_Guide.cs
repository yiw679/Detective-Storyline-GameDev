using System.Collections;
using UnityEngine;
using TMPro;

public class HI_Guide : HelperItem
{
    public string guideText;

    protected override void Help()
    {
        base.Help();
        if (!outline.enabled)
        {
            Subtitle.instance.Show("Walk closer...", 1);
            return;
        }
        Subtitle.instance.Show(guideText, 2);
    }
}
