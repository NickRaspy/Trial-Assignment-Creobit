using UnityEngine;

public class AdventureData : DataManager
{
    [SerializeField] private Timer timer;

    public float RecordTime { get; private set; }

    private Data data = new();
    private void Start() => GetData();
    public override void GetData()
    {
        base.GetData();
        if(JsonUtility.FromJson<Data>(JsonData) != null)
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
