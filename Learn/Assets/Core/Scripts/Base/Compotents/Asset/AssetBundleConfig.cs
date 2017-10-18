using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NEngine.Assets
{
    public class AssetbundleConfig
    {
        public int resourceVersion;
        Dictionary<string, string> bundleAssetsDic = new Dictionary<string, string>(); //资源名 - 包名
        public void AddAssetBundle(string assetname, string bundlename)
        {
            assetname = assetname.ToLower();
            bundlename = bundlename.ToLower();
            if (!bundleAssetsDic.ContainsKey(assetname))
                bundleAssetsDic.Add(assetname, bundlename);
        }
        public void RemoveAssetBundle(string assetname)
        {
            assetname = assetname.ToLower();
            if (bundleAssetsDic.ContainsKey(assetname))
                bundleAssetsDic.Remove(assetname);
        }
        public void ClearAssetbundleDic()
        {
            bundleAssetsDic.Clear();
        }
        public string GetBundlenameByAssetname(string assetname)
        {
            assetname = assetname.ToLower();
            if (bundleAssetsDic.ContainsKey(assetname))
                return bundleAssetsDic[assetname];
            return "";
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(resourceVersion.ToString() + "|");
            foreach (KeyValuePair<string, string> kp in bundleAssetsDic)
            {
                sb.Append(kp.Key + "&" + kp.Value + ",");
            }
            return sb.ToString();
        }

        public static AssetbundleConfig FromStr(string str)
        {
            AssetbundleConfig abc = new AssetbundleConfig();
            string[] strs = str.Split('|');
            if (strs.Length != 2)
                return null;
            int.TryParse(strs[0], out abc.resourceVersion);
            string[] table = strs[1].Split(',');
            string[] kp;
            for (int i = 0; i < table.Length; i++)
            {
                kp = table[i].Split('&');
                if (kp.Length == 2)
                    abc.AddAssetBundle(kp[0], kp[1]);
            }
            return abc;
        }
    }
}
