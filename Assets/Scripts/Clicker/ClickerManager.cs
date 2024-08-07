using System;
using UnityEngine;
using UnityEngine.UI;

namespace CB_TA.Clicker
{
    public class ClickerManager : DataManager
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Button clickButton;

        private Score score;
        private Data data = new();
        private void Start()
        {
            score = new(scoreText);
            clickButton.onClick.AddListener(() => score.Add());
            GetData();
        }
        public override void GetData()
        {
            base.GetData();
            if (JsonUtility.FromJson<Data>(JsonData) != null)
            {
                data = JsonUtility.FromJson<Data>(JsonData);
                score.ScoreAmount = data.score;
            }
        }
        public override void SetData(object data)
        {
            (data as Data).score = score.ScoreAmount;
            base.SetData(data);
        }
        private void OnDisable() => SetData(data);
        [Serializable]
        public class Data
        {
            public int score;
        }
    }
}

