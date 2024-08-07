using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CB_TA
{
    [Serializable]
    public class GameBlock
    {
        public string MainUrl { get { return "https://storage.yandexcloud.net/cbtaunitytest/"; } }
        [Header("Buttons")]
        [SerializeField] private ButtonCollection buttonCollection;
        private Button startButton;
        private Button downloadButton;
        private Button deleteButton;
        private Button cancelButton;
        [SerializeField] private Image loadingImage;

        [Header("Bundle Names")]
        public SceneWithObjectsBundleNames bundleNames;
        #region ASSET_BUNDLES
        public AssetBundle SceneAssetBundle { get; set; }
        public AssetBundle DataAssetBundle { get; set; }
        #endregion

        private Coroutine downloadCoroutine;

        public GameBlock(ButtonCollection buttonCollection, Image loadingImage, SceneWithObjectsBundleNames bundleNames)
        {
            this.buttonCollection = buttonCollection;
            this.loadingImage = loadingImage;
            this.bundleNames = bundleNames;
        }

        public void Init()
        {
            startButton = buttonCollection.startButton;

            downloadButton = buttonCollection.downloadButton;
            downloadButton.onClick.AddListener(() => DownloadSceneWithObjectsFromAssetBundle());

            deleteButton = buttonCollection.deleteButton;

            cancelButton = buttonCollection.cancelButton;
            cancelButton.onClick.AddListener(() => CancelDownload());
        }

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
                if (SceneAssetBundle != null && DataAssetBundle != null) ButtonSetAndEnable();
                else Launcher.instance.errorText.SetActive(true);
            }
            downloadCoroutine = Launcher.instance.StartCoroutine(LoadSceneWithObjects());
        }
        public void CancelDownload()
        {
            if (downloadCoroutine != null)
            {
                Launcher.instance.StopCoroutine(downloadCoroutine);
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
            if (hash == null) yield break;
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
}
[Serializable]
public class ButtonCollection
{
    public Button startButton;
    public Button downloadButton;
    public Button deleteButton;
    public Button cancelButton;
}