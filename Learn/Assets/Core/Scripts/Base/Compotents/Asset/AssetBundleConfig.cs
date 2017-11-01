using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NEngine.Assets
{
    public class AssetbundleConfig
    {
        public AssetbundleConfig()
        {
            bundleAssetsDic = new Dictionary<string, string>();
        }
        Dictionary<string, string> bundleAssetsDic;// = new Dictionary<string, string>(); //资源名 - 包名
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
            System.Text.StringBuilder sss = new System.Text.StringBuilder();
            foreach (var tem in bundleAssetsDic)
            {
                sss.Append(tem.Key + "&" + tem.Value.ToString() + ",");
            }
            return sss.ToString();
        }
        public static AssetbundleConfig FromString(string str)
        {
            AssetbundleConfig con = new AssetbundleConfig();
            string[] strs = str.Split(',');
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i] == "") continue;
                string[] s = strs[i].Split('&');
                if (s.Length == 2)
                {
                    con.AddAssetBundle(s[0], s[1]);
                }
            }
            return con;
        }
    }
}
