using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameBlock : MonoBehaviour
{
    public string MainUrl { get { return "https://storage.yandexcloud.net/cbtaunitytest/"; } }

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button downloadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Image loadingImage;

    [Header("Bundle Names")]
    public SceneWithObjectsBundleNames bundleNames;
    #region ASSET_BUNDLES
    public AssetBundle SceneAssetBundle { get; set; }
    public AssetBundle DataAssetBundle { get; set; }
    #endregion
    private Coroutine downloadCoroutine;
    #region BUTTON_ACTIONS
    public void DownloadSceneWithObjectsFromAssetBundle()
    {
        Launcher.instance.errorText.SetActive(false);
        DownloadAndCancelButtonSwap(true);
        IEnumerator LoadSceneWithObjects()
        {
            if (SceneAssetBundle == null) yield return GetFromWeb(Type.Scene, bundleNames.sceneBundle);
            if (DataAssetBundle == null) yield return GetFromWeb(Type.Data, bundleNames.objectBundle);
            loadingImage.fillAmount = 0;
            DownloadAndCancelButtonSwap(false);
            if(SceneAssetBundle != null && DataAssetBundle != null) ButtonSetAndEnable();
            else Launcher.instance.errorText.SetActive(true);
        }
        downloadCoroutine = StartCoroutine(LoadSceneWithObjects());
    }
    public void CancelDownload()
    {
        if (downloadCoroutine != null)
        {
            StopCoroutine(downloadCoroutine);
            downloadCoroutine = null;
        }
        if (DataAssetBundle != null) DataAssetBundle.Unload(true);
        if (SceneAssetBundle != null) SceneAssetBundle.Unload(true);
        loadingImage.fillAmount = 0;
        DownloadAndCancelButtonSwap(false);
    }
    public void DeleteAction()
    {
        DataAssetBundle.Unload(true);
        SceneAssetBundle.Unload(true);

        Caching.ClearAllCachedVersions(bundleNames.objectBundle);
        Caching.ClearAllCachedVersions(bundleNames.sceneBundle);

        startButton.interactable = false;
        startButton.onClick.RemoveAllListeners();

        deleteButton.interactable = false;
        deleteButton.onClick.RemoveAllListeners();

        downloadButton.interactable = true;
    }
    public void DownloadAndCancelButtonSwap(bool swap)
    {
        downloadButton.gameObject.SetActive(!swap);
        cancelButton.gameObject.SetActive(swap);
    }
    public void ButtonSetAndEnable()
    {
        startButton.onClick.AddListener(() => Launcher.instance.LoadSceneByPath(SceneAssetBundle.GetAllScenePaths()[0]));
        startButton.interactable = true;

        deleteButton.onClick.AddListener(() => DeleteAction());
        deleteButton.interactable = true;

        downloadButton.interactable = false;
    }
    #endregion

    IEnumerator GetFromWeb(Type type, string bundleName)
    {
        //manifest load
        Hash128 hash = new();
        yield return WebFunctions.CheckManifest((value) => hash = value, MainUrl + bundleName);
        if(hash == null) yield break;
        //assetbundle load
        yield return WebFunctions.GetBundle((web) => {
            switch (type)
            {
                case Type.Scene:
                    SceneAssetBundle = DownloadHandlerAssetBundle.GetContent(web);
                    break;
                case Type.Data:
                    DataAssetBundle = DownloadHandlerAssetBundle.GetContent(web);
                    break;
                default:
                    break;
            }
        }, MainUrl + bundleName, hash, loadingImage);
    }
    public enum Type
    {
        Scene, Data
    }
}
