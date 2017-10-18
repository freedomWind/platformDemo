using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NEngine.Assets
{
    public static class AssetHelp
    {
        public static string GetAssetbundleURL(string gamename)//服务器资源地址
        {
            return "";
        }
        public static string GetAssetResource(string gamename)//本地地址
        {
            return "";
        }
        public static AssetBundleManifest GetAssetbundleManifest(string gamename)
        {
            AssetBundleManifest _manifest = null;
            AssetBundle ab = AssetBundle.LoadFromFile(GetAbFloder(gamename));
            if (ab != null) _manifest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            if (_manifest == null) Debug.LogError("manifest wei kong");
            ab.Unload(false);
            return _manifest;
        }
        static string GetAbFloder(string gamename)
        {
            return "";
        }
    }
}
