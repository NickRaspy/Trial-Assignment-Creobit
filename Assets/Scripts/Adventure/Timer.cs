using System;
using UnityEngine;
using UnityEngine.UI;

namespace CB_TA.Adventure
{
    public class Timer
    {
        [SerializeField] private Text text;

        private float time = 0;
        public float TimeSpent
        {
            get { return time; }
            set
            {
                time = value;
                TimeSpan newTime = TimeSpan.FromSeconds(time);
                text.text = string.Format("{0:00}:{1:00}.{2:000}", newTime.Minutes, newTime.Seconds, newTime.Milliseconds);
            }
        }
        public bool IsTiming { get; set; }

        public Timer(Text text)
        {
            this.text = text;
        }

        public void Start() => IsTiming = true;
        public void Update() { if (IsTiming) TimeSpent += Time.deltaTime; }
        public void Restart()
        {
            TimeSpent = 0;
            IsTiming = true;
        }
    }
}
