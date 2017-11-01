using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
namespace NEngine.Assets
{
    public class AssetVersion
    {
        public int assetVersion;
        public string manifestName;
        public Dictionary<string, Hash128> bundlesInfo;
        public AssetVersion()
        {
            assetVersion = -1;
            bundlesInfo = new Dictionary<string, Hash128>();
        }

        public static AssetVersion FromString(string strInfo)
        {
            string[] tem = new string[3];
            tem = strInfo.Split('|');
            if (tem[0] == string.Empty)
            {
                Debug.Log("error ..0");
                return null;
            }
            AssetVersion rinfo = new AssetVersion();
            if (!int.TryParse(tem[0], out rinfo.assetVersion))
            {
                Debug.Log("error ..1");
                return null;
            }
            rinfo.manifestName = tem[1];
            string[] bds = tem[2].Split(',');
            string[] vector = new string[2];
            Dictionary<string, Hash128> dic = new Dictionary<string, Hash128>();
            for (int i = 0; i < bds.Length; i++)
            {
                vector = bds[i].Split('&');
                if (i == bds.Length - 1)
                    continue;
                if (vector.Length != 2)
                {
                    Debug.Log("error .. 3:" + vector.Length);
                    return null;
                }
                if (vector[0] == string.Empty && vector[1] == string.Empty)
                    continue;
                dic.Add(vector[0], Hash128.Parse(vector[1]));
            }
            rinfo.bundlesInfo = dic;
            return rinfo;
        }
        public static void CompareAndGetUpdateList(AssetVersion oldRes, AssetVersion newRes, out Dictionary<string, Hash128> updateList)
        {
            updateList = new Dictionary<string, Hash128>();
            if (oldRes == null || newRes == null)
            {
           //     Debug.LogError("oldRes or newRes is null");
                return;
            }
            Debug.Log("newRes.bundlesInfo:" + newRes.bundlesInfo.Count + "  oldRes.bundlesInfo:" + oldRes.bundlesInfo.Count);
            foreach (KeyValuePair<string, Hash128> tem in newRes.bundlesInfo)
            {
                if (oldRes.bundlesInfo == null)
                {
                    updateList = newRes.bundlesInfo;
                    return;
                }
                if (!oldRes.bundlesInfo.ContainsKey(tem.Key))
                {
                    Debug.Log("新增了一个包：" + tem.Key);
                    updateList.Add(tem.Key, tem.Value);
                }
                else
                {
                    if (tem.Value != oldRes.bundlesInfo[tem.Key]) { Debug.Log("资源包有修改：" + tem.Key); updateList.Add(tem.Key, tem.Value); }
                }
            }
        }
        public static bool CompareBundlesInfo(AssetVersion a, AssetVersion b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            if (a.bundlesInfo.Count != a.bundlesInfo.Count) return false;
            foreach (var tem in a.bundlesInfo)
            {
                if (!b.bundlesInfo.ContainsKey(tem.Key))
                    return false;
                else
                {
                    if (tem.Value != b.bundlesInfo[tem.Key]) return false;
                }
            }
            return true;
        }
        public string ToStr()
        {
            System.Text.StringBuilder sss = new System.Text.StringBuilder();
            sss.Append(assetVersion.ToString() + "|" + manifestName + "|");
            foreach (var tem in bundlesInfo)
            {
                sss.Append(tem.Key + "&" + tem.Value.ToString() + ",");
            }
            return sss.ToString();
        }
    }
}
