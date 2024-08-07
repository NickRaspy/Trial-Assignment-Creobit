using System;
using UnityEngine;
using UnityEngine.UI;

namespace CB_TA.Adventure
{
    public class AdventureManager : DataManager
    {
        [Header("UI")]
        [SerializeField] private Text timerText;
        [SerializeField] private Text recordTimeText;

        [Header("Objects on Scene")]
        [SerializeField] private Finish finish;
        [SerializeField] private ReturnPlayer returner;
        [SerializeField] private PlayerBehavior player;

        private Timer timer;

        public float RecordTime { get; private set; }

        private Data data = new();
        private void Start()
        {
            timer = new Timer(timerText);
            timer.Start();
            finish.OnFinish += FinishGame;
            GetData();
        }
        private void Update()
        {
            timer.Update();
        }

        void FinishGame()
        {
            timer.IsTiming = false;
            TimeSpan recordTime = TimeSpan.FromSeconds(RecordTime);
            if (timer.TimeSpent < RecordTime || RecordTime == 0)
            {
                recordTimeText.text = "New Record!";
                Save();
            }
            else
            {
                recordTimeText.text = "Record: " + string.Format("{0:00}:{1:00}.{2:000}", recordTime.Minutes, recordTime.Seconds, recordTime.Milliseconds);
            }
        }

        public void RestartGame() => timer.Restart();

        public override void GetData()
        {
            base.GetData();
            if (JsonUtility.FromJson<Data>(JsonData) != null)
            {
                data = JsonUtility.FromJson<Data>(JsonData);
                RecordTime = data.time;
            }
        }
        public override void SetData(object data)
        {
            (data as Data).time = timer.TimeSpent;
            base.SetData(data);
        }
        public void Save() => SetData(data);
        public class Data
        {
            public float time;
        }
    }
}
