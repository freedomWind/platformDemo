using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace NEngine.Assets
{
    public static class AssetBundleLoader
    {
        /// <summary>
        /// 从文件中加载ab包
        /// </summary>
        /// <param name="abfullpath"></param>
        /// <param name="callback"></param>
        public static AssetBundle LoadAssetbundle(string abfullpath)
        {
            if (!File.Exists(abfullpath)) return null;
            return AssetBundle.LoadFromFile(abfullpath);
        }

        /// <summary>
        /// 从ab包里加载出资源
        /// </summary>
        /// <param name="ab"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static Object LoadObjectFromAssetbundle(AssetBundle ab,string assetName)
        {
            if (ab != null)
            {
                assetName = assetName.Replace("\\", "/");
                assetName = assetName.Substring(assetName.LastIndexOf('/')+1);//assetName.Replace(Path.GetExtension(assetName), "");
                assetName = assetName.Replace(Path.GetExtension(assetName), "");
                Object[] obs = ab.LoadAllAssets();
                Object oo = null;
                for (int i = 0; i < obs.Length; i++)
                {
                    if (obs[i].name.Equals(assetName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        oo = obs[i];
                        break;
                    }
                }
             //   ab.Unload(false);
                return oo;
            }
            return null;
        }
        public static void LoadBundleFromServer(string url, System.Action<AssetbundleConfig> result)
        {
            App.Ins.AppMono.StartCoroutine(downloadFile(url,_=> {
                result(AssetbundleConfig.FromString(_));
            }));
        }
        public static void LoadAssetversionFromServer(string url, System.Action<AssetVersion> result)
        {
            App.Ins.AppMono.StartCoroutine(downloadFile(url, _ => {
                result(AssetVersion.FromString(_));
            }));
        }
        public static void LoadPreloadFromServer(string url, System.Action<PreloadConfig> result)
        {
            App.Ins.AppMono.StartCoroutine(downloadFile(url, _ => {
                result(PreloadConfig.FromString(_));
            }));
        }
        public static void LoadConfigFromServerSync(string url, System.Action<AssetConfig> result)
        {
            AssetVersion version = null;
            AssetbundleConfig abconfig = null;
            PreloadConfig preload = null;
            LoadAssetversionFromServer(url + "/assetVersion.con", _ => version = _);
            LoadBundleFromServer(url + "bundle.con", _ => abconfig = _);
            LoadPreloadFromServer(url + "preload.con", _ => preload = _);
            result(new AssetConfig("", version, abconfig, preload));
        }
        //同步
        public static void LoadFileFromServerSync(string url, System.Action<byte[]> result)
        {
            App.Ins.AppMono.StartCoroutine(downloadFile(url, result));
        }
        //从服务器更新资
        public static void UpdateAbbagFromServer(string urlpath, IEnumerable<string> downloadList, System.Action<bool> downloadOver = null)
        {
            App.Ins.AppMono.StartCoroutine(downloadAssetbundle(urlpath, downloadList, downloadOver));
        }
        static IEnumerator<YieldInstruction> downloadFile(string url, System.Action<string> callback)
        {
            WWW www = new WWW(url);
            while (!www.isDone)
            {
                yield return new WaitForSeconds(0.01f);
            }
            if (www.error != null) { Debug.LogError(string.Format("load assetResourceinfo from {0} error:{1}", url, www.error)); yield break; }
            url = url.Replace("/", "\\");
            string filename = url.Substring(url.LastIndexOf("\\") + 1);
            if (callback != null) callback(www.text);
        }
        static IEnumerator<YieldInstruction> downloadFile(string url, System.Action<byte[]> callback)
        {
            WWW www = new WWW(url);
            //   WWW www = new WWW(@url);
            while (!www.isDone)
            {
                yield return new WaitForSeconds(0.01f);
            }
            if (www.error != null) { Debug.LogError(string.Format("load assetResourceinfo from {0} error:{1}", url, www.error)); yield break; }
            url = url.Replace("/", "\\");
            string filename = url.Substring(url.LastIndexOf("\\") + 1);
          //  saveAsset(filename, www.bytes);
            if (callback != null) callback(www.bytes);
        }       
        static IEnumerator<YieldInstruction> downloadAssetbundle(string path, IEnumerable<string> downloadList, System.Action<bool> downloadOver = null)
        {
            WWW www = null;
            string pathex = path + "/";
            bool isSuccess = false;
            string fullname = "";
            if (downloadList == null)
                isSuccess = true;
            foreach (var tem in downloadList)
            {
                fullname = pathex + tem;
                fullname = fullname.Replace("\\", "/");

                isSuccess = false;
                www = new WWW(@fullname);
                Debug.Log("download path:" + fullname);
                while (!www.isDone)
                {
                    Debug.Log("正在下载：" + tem);
                    yield return new WaitForSeconds(0.02f);
                }
                Debug.Log(string.Format("下载 {0} : {1:N1}%", tem, (www.progress * 100)));
                if (www.error != null)
                {
                    if (downloadOver != null) downloadOver(false);
                    Debug.LogError("www error:" + www.error.ToString());
                }

                if (www.assetBundle != null)
                {
                    isSuccess = true;
                    saveAsset(tem, www.bytes);
                    www.assetBundle.LoadAllAssets();
                    www.assetBundle.Unload(false);
                }
            }
            if (www != null) www.Dispose();
            if (downloadOver != null)
                downloadOver(isSuccess);
        }
        static IEnumerator<YieldInstruction> downloadManifest(string filename, System.Action<AssetBundleManifest> manifestCallback)
        {
            filename = filename.Replace("\\", "/");
            using (WWW www = new WWW(@filename))
            {
                while (!www.isDone)
                {
                    Debug.Log("正在下载：" + filename);
                    yield return new WaitForSeconds(0.02f);
                }
                Debug.Log(string.Format("下载 {0} : {1:N1}%", filename, (www.progress * 100)));
                if (www.assetBundle != null)
                {
                    string subname = filename.Substring(filename.LastIndexOf("/") + 1);
                    Debug.Log("--------------subname:" + subname);
                    saveAsset(subname, www.bytes);
                    
                    AssetBundleManifest am = www.assetBundle.LoadAsset("AssetBundleManifest", typeof(AssetBundleManifest)) as AssetBundleManifest;
                    www.assetBundle.Unload(false);
                    if (manifestCallback != null) manifestCallback(am);
                }
            }
        }
        static void saveAsset(string name, byte[] bytes)
        {
            FileUtil.SaveFile(name, bytes);
        }
    }
    public static class Downloader
    {
        public static void DownLoaderFile(string url, System.Action<byte[]> callback)
        {
            App.Ins.AppMono.StartCoroutine(downloadFile(url, callback));
        }
        static IEnumerator<YieldInstruction> downloadFile(string url, System.Action<byte[]> callback)
        {
            WWW www = new WWW(@url);
            while (!www.isDone)
            {
                yield return new WaitForSeconds(0.01f);
            }
            if (www.error != "") { Debug.LogError(string.Format("load assetResourceinfo from {0} error:{1}", url, www.error)); yield break; }
            url = url.Replace("/", "\\");
            string filename = url.Substring(url.LastIndexOf("\\") + 1);
            if (callback != null) callback(www.bytes);
        }
    }
    public static class AssetbundleHelp
    {
        private static Dictionary<string, AssetBundle> rememberDic = new Dictionary<string, AssetBundle>();
        public static AssetBundle LoadFromFile(string bundleName)
        {
            AssetBundle ab = null;
            if (!rememberDic.ContainsKey(bundleName))
            {
                ab = AssetBundle.LoadFromFile(bundleName);
                rememberDic.Add(bundleName, ab);
            }
            return rememberDic[bundleName];
        }
        public static void AddAssetBundle(string bundleName, AssetBundle ab)
        {
            if (!rememberDic.ContainsKey(bundleName))
            {
                rememberDic.Add(bundleName, ab);
            }
        }
        public static void UnLoadBundles()
        {
            foreach (KeyValuePair<string, AssetBundle> kp in rememberDic)
            {
                kp.Value.Unload(false);
            }
            rememberDic.Clear();
            Debug.Log("abbundle包卸载完毕");
         //   AppFacade.Ins.Log(BugType.log, "abbundle包卸载完毕");
        }
    }
}