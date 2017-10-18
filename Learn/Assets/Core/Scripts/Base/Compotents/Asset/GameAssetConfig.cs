using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NEngine.Assets
{
    /// <summary>
    /// 游戏资源配置
    /// </summary>
    public class GameAssetConfig
    {
        private List<string> GameAssetList;                              //游戏资源
        private Dictionary<string, List<string>> SceneAssetDic;         //场景资源
        public GameAssetConfig()
        {
            GameAssetList = new List<string>();
            SceneAssetDic = new Dictionary<string, List<string>>();
            GameAssetList.Add("egg1");
            GameAssetList.Add("egg2");
            SceneAssetDic.Add("scene1", new List<string>() { "a", "b" });
            SceneAssetDic.Add("scene2", new List<string>() { "c", "d" });
        }
        public GameAssetConfig(List<string> g, Dictionary<string, List<string>> s)
        {
            GameAssetList = g;
            SceneAssetDic = s;
        }

        public void ClearGameAssetList()
        {
            GameAssetList.Clear();
        }

        public List<string> GetGameassetList()
        {
            return GameAssetList;
        }

        public List<string> GetAssetListBySceneName(string sceneName)
        {
            if (SceneAssetDic.ContainsKey(sceneName))
                return SceneAssetDic[sceneName];
            return null;
        }

        public static GameAssetConfig FromStr(string str)
        {
            List<string> ga = new List<string>();
            Dictionary<string, List<string>> sa = new Dictionary<string, List<string>>();
            string[] strs = str.Split(';');
            if (strs.Length == 0) { Debug.LogError("1"); return null; }
            for (int k = 0; k < strs.Length; k++)
            {
                List<string> assetlist = new List<string>();
                string[] gaAssets = strs[k].Split('|');
                if (gaAssets.Length != 2) continue;
                string[] assetArray = gaAssets[1].Split(',');
                for (int i = 0; i < assetArray.Length; i++)
                    assetlist.Add(assetArray[i]);
                if (k == 0)
                {
                    if (gaAssets[0] != "gameassets") { Debug.LogError("2"); return null; }
                    ga = assetlist;
                }
                else
                {
                    sa.Add(gaAssets[0], assetlist);
                }
            }
            GameAssetConfig config = new GameAssetConfig(ga, sa);
            return config;
        }

        public string ToStr()
        {
            string str = "gameassets|";
            if (GameAssetList.Count != 0)
                str += GameAssetList[0];
            for (int i = 1; i < GameAssetList.Count; i++)
            {
                str += ",";
                str += GameAssetList[i];
            }
            str += ";";
            if (SceneAssetDic.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> kp in SceneAssetDic)
                {
                    str += kp.Key + "|";
                    if (kp.Value.Count != 0)
                    {
                        str += kp.Value[0];
                    }
                    for (int i = 1; i < kp.Value.Count; i++)
                    {
                        str += ",";
                        str += kp.Value[i];
                    }
                    str += ";";
                }

            }
            return str;
        }
    }
}
