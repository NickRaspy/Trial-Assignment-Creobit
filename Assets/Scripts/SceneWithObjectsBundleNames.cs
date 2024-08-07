using UnityEngine;

namespace CB_TA
{
    [CreateAssetMenu(fileName = "SceneWithObjectsBundleNames", menuName = "Launcher/SceneWithObjectsBundleNames", order = 1)]
    public class SceneWithObjectsBundleNames : ScriptableObject
    {
        public string objectBundle;
        public string sceneBundle;
    }
}