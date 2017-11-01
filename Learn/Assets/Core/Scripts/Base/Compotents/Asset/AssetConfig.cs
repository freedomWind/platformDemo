using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEngine.Assets
{
    /// <summary>
    /// 资源配置文件
    /// </summary>
    public class AssetConfig
    {
        public static string getAssetbundleFloderPath(string gName)            //ab包目录
        {
            return "";
        }
        public static string getAssetConfigPath_local(string gName)             //本地资源配置文件目录
        {
            return "";
        }
        public static string getAssetConfigPath_ser(string gName)               //服务器端下载存放目录
        {
            return "";
        }
        public static string getAssetUrl(string gName)                          //服务器端资源地址
        {
            return "";
        }

        private string gName;
        public AssetConfig(string gamename,AssetVersion aversion,AssetbundleConfig abconfig,PreloadConfig preloadConfig)
        {
            this.gName = gamename;
            this.assetVersion = aversion;
            this.abConfig = abconfig;
            this.preloadConfig = preloadConfig;
        }
        AssetVersion assetVersion;
        AssetbundleConfig abConfig;
        PreloadConfig preloadConfig;
       // GameAssetConfig assetConfig;

        /// <summary>
        /// 通过资源路径名得到ab包名
        /// </summary>
        /// <param name="assetname"></param>
        /// <returns></returns>
        public string GetAbgNameByAssetname(string assetname)
        {
            assetname = assetname.ToLower();
            return abConfig.GetBundlenameByAssetname(assetname);
        }
        public AssetBundle GetAssetbundleByAssetname(string assetname)
        {
            try
            {
                string finalPath = Application.streamingAssetsPath + "/" + GetAbgNameByAssetname(assetname);
                if (!System.IO.File.Exists(finalPath))
                    return null;
                return AssetBundle.LoadFromFile(finalPath);
            }
            catch { Debug.LogError(string.Format("assetbundle {0} load error",assetname));  return null; }
        }
        public string[] GetPreloadArray()
        {
            try
            {
                return this.preloadConfig.ToArray();
            }
            catch { return null; }
        }
        private AssetBundleManifest _manifest = null;
        public AssetBundleManifest Manifest
        {
            get
            {
                if (_manifest == null)
                {
                    _manifest = AssetHelp.GetAssetbundleManifest(gName);
                }
                return _manifest;
            }
        }
        public void SetManifest(AssetBundleManifest am)
        {
            _manifest = am;
        }
        public AssetBundle LoadAssetBundle(string name) 
        {
            return null;
        }
    }
}

