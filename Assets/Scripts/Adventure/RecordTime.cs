using System;
using UnityEngine;
using UnityEngine.UI;

public class RecordTime : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private AdventureData adventureData;
    private void OnEnable()
    {
        timer.IsTiming = false;
        TimeSpan recordTime = TimeSpan.FromSeconds(adventureData.RecordTime);
        if(timer.TimeSpent < adventureData.RecordTime)
        {
            GetComponent<Text>().text = "New Record!";
            adventureData.Save();
        }
        else
        {
            GetComponent<Text>().text = "Record: " + string.Format("{0:00}:{1:00}.{2:000}", recordTime.Minutes, recordTime.Seconds, recordTime.Milliseconds);
        }
    }
}
