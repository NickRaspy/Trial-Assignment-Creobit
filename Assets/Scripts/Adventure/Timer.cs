using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float time;
    public float TimeSpent
    {
        get { return time; }
        set 
        { 
            time = value;
            TimeSpan newTime = TimeSpan.FromSeconds(time);
            GetComponent<Text>().text = string.Format("{0:00}:{1:00}.{2:000}", newTime.Minutes, newTime.Seconds, newTime.Milliseconds);
        }
    }
    public bool IsTiming {  get; set; }
    private void Start() => IsTiming = true;
    void Update()
    {
        if (IsTiming) TimeSpent += Time.deltaTime;
    }
}
