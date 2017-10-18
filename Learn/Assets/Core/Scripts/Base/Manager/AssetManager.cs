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
        public AssetConfig assetConfig { get { return _configDic[GameManager.NowRunning.Name]; } }

    }
}
