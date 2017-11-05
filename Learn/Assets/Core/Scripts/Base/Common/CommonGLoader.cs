using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine.Game;
using NEngine.Assets;
using System;

/// <summary>
/// 通用启动加载
/// </summary>
public class CommonGLoader : IGameLoad
{
    private GameEnum curGame;
    private List<string> updateList;
    public CommonGLoader(GameEnum gamename)
    {
        this.curGame = gamename;
        updateList = new List<string>();
    }
    public override bool CheckGameUpdate()
    {
        return GameConfig.IsNewVersion(GameConfig.GetGameVersion(curGame, false), GameConfig.GetGameVersion(curGame));
    }
    public override void DoGameUpdate(Action callback)
    {
        base.DoGameUpdate(callback);
    }
    public override void CheckAndUpdateConfig(string url, System.Action<byte[]> callback)
    {
        if (!Util.isEditor)
        {
            #region  //压缩包形式
            base.CheckAndUpdateConfig(@"c:/virtualSer/learn/" + curGame + "/_config/_con.rar", _ =>
            {
                //解压然后得到对应的config， bundle, assetversion, preload 同时存储在本地
            });
            #endregion
        }
        App.GetMgr<AssetManager>().AddAssetConfig(curGame.ToString(), LoadLocalConfig(curGame.ToString()));
    }
    public override bool CheckAssetsUpdate(out IEnumerable<string> pullist)
    {
        pullist = null;
        updateList.Clear();
        Dictionary<string, Hash128> dic = new Dictionary<string, Hash128>();
        bool f = GameConfig.IsNewAssets(GameConfig.GetAssetVersion(curGame, false), GameConfig.GetAssetVersion(curGame), out dic);
        if (f) //
        {
            pullist = dic.Keys;
        }
        Debug.Log("资源更新否：" + f.ToString());
        return f;
    }
    public override void PullAssets(IEnumerable<string> pulllist)
    {
        Debug.Log("从服务器更新资源");
    //    AssetBundleLoader.UpdateAbbagFromServer(AssetConfig.getAssetUrl(curGame.ToString()), pulllist, OnAssetsLoadOver);
    }
    void OnAssetsLoadOver(bool flag)
    {
        if (flag)
        {

        }
    }
    public override void PreLoadAssets()
    {
        AssetConfig con = App.GetMgr<AssetManager>().GetAssetConfig(curGame.ToString());
        if (con != null)
        {
            string[] arr = con.GetPreloadArray();
            if (arr == null) return;
            UnityEngine.Object ob = null;
            for (int i = 0; i < arr.Length; i++)
            {
                if (!AssetUseRecorder.ExistAsset(arr[i]))
                {
                    ob = App.GetMgr<ResourceManager>().LoadAsset(arr[i]);
                    if (ob != null)
                        AssetUseRecorder.AddToStatic(arr[i], ob);   //预加载的资源生命周期完整保存
                }
            }
        }

    }
    public override bool Connect()
    {
        Texture2D tex = App.GetMgr<ResourceManager>().LoadAsset<Texture2D>("StartLoading_Res/startbg.jpg");
        Debug.Log("texname = " + tex.name);
    //    tex = App.GetMgr<ResourceManager>().LoadAsset<Texture2D>("StartLoading_Res/startbg.jpg");
        return true;
    }
}
