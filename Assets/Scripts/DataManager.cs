using System.IO;
using UnityEngine;

namespace CB_TA
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] private string dataFolderName;
        public string JsonData { get; set; }
        private string DataFolderPath
        {
            get
            {
                if (!string.IsNullOrEmpty(dataFolderName)) return Path.Combine(Application.persistentDataPath, dataFolderName);
                else return Path.Combine(Application.persistentDataPath, gameObject.scene.name);
            }
        }
        public virtual void GetData()
        {
            if (File.Exists(Path.Combine(DataFolderPath, "data.json"))) JsonData = File.ReadAllText(Path.Combine(DataFolderPath, "data.json"));
            else return;
        }
        public virtual void SetData(object data)
        {
            if (!Directory.Exists(DataFolderPath)) Directory.CreateDirectory(DataFolderPath);
            string dataToSave = JsonUtility.ToJson(data);
            File.WriteAllText(Path.Combine(DataFolderPath, "data.json"), dataToSave);
        }
    }
}
