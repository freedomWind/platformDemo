using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace NEngine.Assets
{
    public static class AssetBundleLoader
    {
        /*
        public static void LoadAssetsASync(string abfullpath, System.Action<AssetBundle> callback)
        {
            if (!File.Exists(abfullpath)) return;
            LoadAssetbundleSync(abfullpath, callback);
        }
        public static void LoadFileFromServer(string url, System.Action<string> result)
        {
            //AppFacade.Ins.gMono.StartCoroutine(loadFileSync(url, result));
        }
       
        //从服务器更新资源包
        public static void UpdateFromServer(string urlpath, Dictionary<string, Hash128> downloadList, System.Action<bool> downloadOver = null)
        {
            AppFacade.Ins.gMono.StartCoroutine(downloadAssetbundle(urlpath, downloadList, downloadOver));
        }
        //更新manifest
        public static void UpdateManifestFromServer(string url, System.Action<AssetBundleManifest> manifestCallback)
        {
            url = url.Replace("\\", "/");
            string manifest = url.Substring(url.LastIndexOf("/") + 1);
            string finalname = url + "/" + manifest;
            Debug.Log("finalname---" + finalname);
            AppFacade.Ins.gMono.StartCoroutine(downloadManifest(finalname, (AssetBundleManifest am) =>
            {
                if (am != null) { manifestCallback(am); }
            }));
        }
        // 协程实现
        static IEnumerator<YieldInstruction> LoadAsyncCoroutine(string path, System.Action<AssetBundle> callback)
        {
            AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(path);
            yield return abcr;
            //
            Object[] obs = abcr.assetBundle.LoadAllAssets();

            callback(abcr.assetBundle);
        }
     //   private static List<string> rememberList = new List<string>();
        static AssetBundle LoadSyncCoroutine(string path)
        {
            Debug.Log("path - " + path);

            AssetBundle ab = AssetbundleHelp.LoadFromFile(path);
            if (ab == null)
            {
                Debug.LogError("assetbundle loadFromFile error:");
            }
            path = path.Replace("/", "\\");
            string localpath = AssetBundleInfo.assetPath_local;
            localpath = localpath.Replace("/", "\\");
            string assetbundlename = path.Replace(localpath + "\\", "");
            Debug.Log("assetbundlename:" + assetbundlename + " localpath:" + localpath);
            string[] dependencies = AssetBundleInfo.Manifest.GetAllDependencies(assetbundlename);
            for (int i = 0; i < dependencies.Length; i++)
            {
                Debug.Log("dependencies:" + dependencies[i]);
                //if(!rememberList.Contains(localpath + "\\" + dependencies[i]))
                AssetBundle child = AssetbundleHelp.LoadFromFile(localpath + "\\" + dependencies[i]);
                if (child != null) child.LoadAllAssets();
            }
            return ab;
        }
        static void LoadFromFileDependices(string fullname)
        {
            string dir = fullname.Substring(0, fullname.LastIndexOf('/')) + "/";
            string filename = fullname.Substring(fullname.LastIndexOf('/') + 1);
            AssetBundle ab = AssetbundleHelp.LoadFromFile(fullname);
            string[] dependencies = AssetBundleInfo.Manifest.GetAllDependencies(filename);
            for (int i = 0; i < dependencies.Length; i++)
            {
                AssetBundle child = AssetbundleHelp.LoadFromFile(dir + dependencies[i]);
                child.LoadAllAssets();
            }
            ab.LoadAllAssets();
        }
        static void LoadAssetbundleSync(string assetbundlePath, System.Action<AssetBundle> callback)
        {
            callback(LoadSyncCoroutine(assetbundlePath));
        }
        static IEnumerator<YieldInstruction> loadFileSync(string url, System.Action<string> callback)
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
            saveAsset(filename, www.bytes);
            if (callback != null) callback(www.text);
        }
        static void LoadAssetbundleAsync(string finalPath, System.Action<UnityEngine.AssetBundle> callback)
        {
            //  GameLogic.Ins.StartCoroutine(LoadAsyncCoroutine(finalPath, callback));
        }
        static IEnumerator<YieldInstruction> downloadAssetbundle1(string path, Dictionary<string, Hash128> downloadList, System.Action<bool> downloadOver = null)
        {
            string pathex = "file:///" + path + "/";
            bool isSuccess = false;
            string fullname = "";
            if (downloadList == null || downloadList.Count == 0)
                isSuccess = true;
            foreach (KeyValuePair<string, Hash128> tem in downloadList)
            {
                fullname = pathex + tem.Key;
                fullname = fullname.Replace("\\", "/");

                isSuccess = false;
                WWW www = new WWW(@fullname);
                Debug.Log("download path:" + fullname);
                while (!www.isDone)
                {
                    Debug.Log("正在下载：" + tem.Key);
                    yield return new WaitForSeconds(0.02f);
                }
                Debug.Log(string.Format("下载 {0} : {1:N1}%", tem.Key, (www.progress * 100)));
                if (www.error != null)
                {
                    if (downloadOver != null) downloadOver(false);
                    Debug.LogError("www error:" + www.error.ToString());
                    continue;
                }
                if (www.assetBundle != null)
                {

                    saveAsset(tem.Key, www.bytes);
                    //     www.assetBundle.LoadAllAssetsAsync();
                    isSuccess = true;

                    //    www.assetBundle.Unload(false);
                }
                // www.Dispose();
                //if (www.assetBundle != null)
                //{
                //    isSuccess = true;
                //    saveAsset(tem.Key, www.bytes);
                //    www.assetBundle.LoadAllAssets();
                //    www.assetBundle.Unload(false);
                //}
            }
            if (downloadOver != null)
                downloadOver(isSuccess);
        }
        static IEnumerator<YieldInstruction> downloadAssetbundle(string path, List<string> downloadList, System.Action<bool> downloadOver = null)
        {
            WWW www = null;
            string pathex = path + "/";
            bool isSuccess = false;
            string fullname = "";
            if (downloadList == null || downloadList.Count == 0)
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
            name = name.Replace("\\", "/");
            string path = AssetBundleInfo.assetPath_local + "/" + name;
            Debug.Log("save asset path:" + path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string dir = path.Substring(0, path.LastIndexOf("/"));
            Debug.Log("what's the dir:" + dir);
            if (!Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            FileStream fs = File.Create(path);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
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
    */
    }
}