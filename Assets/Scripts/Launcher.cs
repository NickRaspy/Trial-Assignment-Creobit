using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
namespace CB_TA
{
    public class Launcher : MonoBehaviour
    {
        #region SINGLETON
        public static Launcher instance;
        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance == this) Destroy(gameObject);
        }
        #endregion
        [SerializeField] private List<GameBlock> gameBlocks;
        [SerializeField] private GameObject wholeMainMenu;
        [SerializeField] private GameObject exitButton;
        [SerializeField] private GameObject updateScreen;

        [SerializeField] private AssetLabelReference refer;
        [SerializeField] private Image test;
        [SerializeField] private Image image;
        public GameObject errorText;

        private void Start()
        {
            gameBlocks.ForEach(gb => gb.Init());
            if (Caching.cacheCount == 0) return;
            StartCoroutine(WebFunctions.CheckInternetConnection((connected) =>
            {
                if (connected) StartCoroutine(UpdateCheck());
                else gameBlocks.ForEach(b => LoadAllBundlesFromCache(b));
            }));
        }
        IEnumerator UpdateCheck()
        {
            updateScreen.SetActive(true);
            foreach (GameBlock gb in gameBlocks)
            {
                int loadedAmount = 0;
                for (int i = 0; i < 2; i++)
                {
                    Hash128 hash = new();
                    yield return WebFunctions.CheckManifest((value) => hash = value, gb.MainUrl + (i == 0 ? gb.bundleNames.sceneBundle : gb.bundleNames.objectBundle));
                    List<Hash128> hashes = new();
                    Caching.GetCachedVersions(i == 0 ? gb.bundleNames.sceneBundle : gb.bundleNames.objectBundle, hashes);
                    if (hashes.Contains(hash))
                    {
                        LoadBundleFromCache(gb, i == 0 ? GameBlock.Type.Scene : GameBlock.Type.Data);
                        loadedAmount++;
                    }
                }
                if (loadedAmount == 2) gb.ButtonSetAndEnable();
            }
            updateScreen.SetActive(false);
        }
        #region BUNDLE_LOAD
        void LoadBundleFromCache(GameBlock gameBlock, GameBlock.Type type)
        {
            try
            {
                switch (type)
                {
                    case GameBlock.Type.Scene:
                        gameBlock.SceneAssetBundle = AssetBundle.LoadFromFile(Directory.GetDirectories(Path.Combine(Caching.defaultCache.path, gameBlock.bundleNames.sceneBundle)).Last() + "/__data");
                        break;
                    case GameBlock.Type.Data:
                        gameBlock.DataAssetBundle = AssetBundle.LoadFromFile(Directory.GetDirectories(Path.Combine(Caching.defaultCache.path, gameBlock.bundleNames.objectBundle)).Last() + "/__data");
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }
        }
        void LoadAllBundlesFromCache(GameBlock gameBlock)
        {
            try
            {
                if (Directory.Exists(Path.Combine(Caching.defaultCache.path, gameBlock.bundleNames.sceneBundle))
                && Directory.Exists(Path.Combine(Caching.defaultCache.path, gameBlock.bundleNames.objectBundle)))
                {
                    gameBlock.SceneAssetBundle = AssetBundle.LoadFromFile(Directory.GetDirectories(Path.Combine(Caching.defaultCache.path, gameBlock.bundleNames.sceneBundle)).Last() + "/__data");
                    gameBlock.DataAssetBundle = AssetBundle.LoadFromFile(Directory.GetDirectories(Path.Combine(Caching.defaultCache.path, gameBlock.bundleNames.objectBundle)).Last() + "/__data");
                    gameBlock.ButtonSetAndEnable();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }
        }
        #endregion

        #region SCENE
        public void LoadSceneByPath(string path)
        {
            IEnumerator SceneChange()
            {
                CloseMainMenu();
                yield return SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(SceneManager.GetSceneByPath(path));
            }
            StartCoroutine(SceneChange());
        }
        public void LoadSceneByName(string name)
        {
            IEnumerator SceneChange()
            {
                CloseMainMenu();
                yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
            }
            StartCoroutine(SceneChange());
        }
        public void CloseMainMenu()
        {
            wholeMainMenu.SetActive(false);
            exitButton.SetActive(true);
        }
        public void ReturnToMainMenu()
        {
            exitButton.SetActive(false);
            IEnumerator Return()
            {
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
                wholeMainMenu.SetActive(true);
            }
            StartCoroutine(Return());
        }
        #endregion
    }
}