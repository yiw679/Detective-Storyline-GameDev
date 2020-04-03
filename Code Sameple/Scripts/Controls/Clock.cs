using UnityEngine;
using System;
using TMPro;

public class Clock : MonoBehaviour
{
    private TextMeshProUGUI textClock;

    void Awake()
    {
        textClock = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        DateTime time = GlobalEventController.instance.curTime;
        string hour = LeadingZero(time.Hour);
        string minute = LeadingZero(time.Minute);
        textClock.text = "Time: " + hour + ":" + minute;
    }
    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}