using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneWithObjectsBundleNames", menuName = "Launcher/SceneWithObjectsBundleNames", order = 1)]
public class SceneWithObjectsBundleNames : ScriptableObject
{
    public string objectBundle;
    public string sceneBundle;
}
