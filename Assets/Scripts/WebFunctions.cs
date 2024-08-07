using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace CB_TA
{
    public static class WebFunctions
    {
        public static int AvailableSpace
        {
            get
            {
                var logicalDrive = Path.GetPathRoot(Caching.defaultCache.path);
                return SimpleDiskUtils.DiskUtils.CheckAvailableSpace(logicalDrive);
            }
        }
        public static IEnumerator CheckManifest(Action<Hash128> action, string fileURL)
        {
            UnityWebRequest web = UnityWebRequest.Get(fileURL + ".manifest");
            yield return web.SendWebRequest();
            if (web.result != UnityWebRequest.Result.Success) { Debug.LogError("Failed to get AssetBundle Manifest!"); yield break; }
            var hashRow = web.downloadHandler.text.ToString().Split("\n".ToCharArray())[5];
            Hash128 hash = Hash128.Parse(hashRow.Split(':')[1].Trim());
            action(hash);
        }
        public static IEnumerator GetBundle(Action<UnityWebRequest> action, string fileURL, Hash128 hash, Image loadingImage = null)
        {
            bool isEnoughSpace = true;
            yield return CheckAvailableSpace_URL(fileURL, (available) => isEnoughSpace = available);
            if (!isEnoughSpace)
            {
                Debug.LogError("Not enough space! (or Unity wasn't able to get file size)");
                yield break;
            }
            UnityWebRequest web = UnityWebRequestAssetBundle.GetAssetBundle(fileURL, hash, 0);
            AsyncOperation operation = web.SendWebRequest();
            while (!operation.isDone)
            {
                if (loadingImage != null) loadingImage.fillAmount = operation.progress;
                yield return null;
            }
            if (web.result != UnityWebRequest.Result.Success) { Debug.LogError("Failed to get AssetBundle!"); yield break; }
            action(web);
        }
        public static IEnumerator CheckAvailableSpace_URL(string fileURL, Action<bool> action)
        {
            UnityWebRequest head = UnityWebRequest.Head(fileURL);
            yield return head.SendWebRequest();
            if (head.result != UnityWebRequest.Result.Success) { Debug.LogError("Failed to get size info from AssetBundle!"); action(false); yield break; }
            float fileSize = Convert.ToInt64(head.GetResponseHeader("Content-Length")) / 1048576f;
            action(AvailableSpace > fileSize);
        }
        public static void CheckAvailableSpace_Value(long value, Action<bool> action)
        {
            float fileSize = value / 1048576f;
            action(AvailableSpace > fileSize);
        }
        public static IEnumerator CheckInternetConnection(Action<bool> action)
        {
            UnityWebRequest web = new("https://google.com");
            yield return web.SendWebRequest();
            if (web.result != UnityWebRequest.Result.Success)
            {
                action(false);
            }
            else
            {
                action(true);
            }
        }
    }
    public class ResourceLocationWithSize
    {
        public IResourceLocation resourceLocation;
        public long size;
        public ResourceLocationWithSize(IResourceLocation resourceLocation, long size)
        {
            this.resourceLocation = resourceLocation;
            this.size = size;
        }
    }
}
