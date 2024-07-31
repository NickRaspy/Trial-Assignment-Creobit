using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ClickerData : DataManager
{
    [SerializeField] private Score score;
    private Data data = new();
    private void Start()
    {
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
