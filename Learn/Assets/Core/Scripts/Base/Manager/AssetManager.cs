using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine.Game;
namespace NEngine.Assets
{
    public class AssetManager
    {
        Dictionary<string, AssetConfig> _configDic;
        public AssetManager()
        {
            _configDic = new Dictionary<string, AssetConfig>();
        }
        public void AddAssetConfig(string name, AssetConfig config)
        {
            if (_configDic.ContainsKey(name))
            {
                _configDic.Remove(name);
            }
            _configDic.Add(name, config);
        }
        public AssetConfig GetAssetConfig(string gamename)
        {
            AssetConfig con = null;
            _configDic.TryGetValue(gamename, out con);
            return con;
        }
        private AssetBundle GetBundle(string assetname)
        {
            string now = GameManager.NowRunning.Name;
            AssetBundle ab = GetAssetConfig(now).GetAssetbundleByAssetname(assetname);
            if (ab == null)
            {
                string[] gnames = System.Enum.GetNames(typeof(GameEnum));
                for (int i = 0; i < gnames.Length; i++)
                {
                    if (gnames[i] == now)
                        continue;
                    ab = GetAssetConfig(gnames[i]).GetAssetbundleByAssetname(assetname);
                    if (ab == null) continue;
                }
            }
            return ab;
        }
        public UnityEngine.Object LoadAssetFromBundle(string assetname)
        {
          //  try
           // {
                AssetBundle ab = GetBundle(assetname);
                return AssetBundleLoader.LoadObjectFromAssetbundle(ab, assetname);
        //    }
      //      catch { return null; }
        }
    }
}
