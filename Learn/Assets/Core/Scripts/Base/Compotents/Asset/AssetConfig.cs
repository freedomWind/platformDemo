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
        private string gName;
        public AssetConfig(string gamename)
        {
            this.gName = gamename;
            assetVersion = new AssetVersion();
        }
        AssetVersion assetVersion;
        AssetbundleConfig abConfig;
        GameAssetConfig assetConfig;

        /// <summary>
        /// 通过资源路径名得到ab包名
        /// </summary>
        /// <param name="assetname"></param>
        /// <returns></returns>
        public string GetAbgNameByAssetname(string assetname)
        {
            return "";
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

