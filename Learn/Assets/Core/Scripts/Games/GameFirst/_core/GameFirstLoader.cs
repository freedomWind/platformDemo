using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine.Game;
using NEngine.Assets;
using System;
using YZL.Compress.UPK;
/// <summary>
/// 游戏初始加载器
/// 更新配置文件（版本，所有资源配置信息）
/// 游戏dll热更新
/// 更新游戏资源
/// 
/// </summary>
public class GameFirstLoader : IGameLoad
{
    public static string appResPath = Util.appDataPath + "/Res";                 //app资源目录
    string appConfigPath_new = Util.appDataPath + "/serCon";                     //app配置文件（最新）
    string appConfigPath_old = Util.appDataPath + "/localCon";                   //app本地配置文件
    string appHotfixPath = Util.appDataPath + "/hotfix";                         //app热更新目录
    string appversion_config = "Conn/appVersion.config";

    string serConfigAddr               //一个压缩包地址，包含所有游戏版本文件，所有游戏配置文件
    {  
        get {
            if (Util.isEditor)
            {
                return Application.dataPath + "/../virtualser/conn/appConfig.upk";
            }
            else
                return ""; 
        }
    }
    string serResAddr
    {
        get
        {
            if (Util.isEditor)
            {
                return Application.dataPath + "/../virtualser/Res";
            }
            else
                return "";
        }
    }

    public override bool CheckGameUpdate()
    {
        //游戏启动过程必须更新配置信息
        return true;
    }
    public override void DoGameUpdate(Action callback)
    {
        //更新版本配置信息 upk包  
        Downloader.DownLoaderFile(serConfigAddr, _ =>
        {
            FileUtil.ClearDirectory(appConfigPath_new);
            FileUtil.SaveFile(appConfigPath_new + "/appConfig.upk", _);
            UPKFolder.UnPackFolder(appConfigPath_new + "/appConfig.upk", appConfigPath_new + "/", null);
            System.IO.File.Delete(appConfigPath_new + "/appConfig.upk");
            // 比较gameversion 而后更新dll，下载新的ab包  （一次性更新所有dll）
            compareAndUpdate();
            //替换配置信息
            replaceConfig();
            initAssetManager();
        });
    }
    
    public override bool CheckAssetsUpdate(out IEnumerable<string> updatelist)
    {
        updatelist = null;
        if (Util.isEditor)
            return false;
        return true;
    }
    public override void CheckAndUpdateConfig(string url, Action<byte[]> loaderOver)
    {
        base.CheckAndUpdateConfig(url, loaderOver);
    }
    string[] getUpdatelist()
    {
        string[] uplist = null;
        string[] strs = System.Enum.GetNames(typeof(NEngine.Game.GameEnum));
        for (int i = 0; i < strs.Length; i++)
        {
            AssetVersion ver1 = AssetVersion.FromString(FileUtil.ReadFromFile(appConfigPath_new + "/Conn/" + strs[i] + "/assetVersion.config"));
            AssetVersion ver2 = AssetVersion.FromString(FileUtil.ReadFromFile(appConfigPath_old + "/Conn/" + strs[i] + "/assetVersion.config"));
            if (ver1 == null || ver2 == null)
            {
                Debug.LogError(string.Format("{0} assetversion is null",strs[i])); continue;
            }
            uplist = AssetVersion.CompareAndGetUpdateList(ver2, ver1);
        }
        return uplist;
    }
    void initAssetManager()
    {
        string[] strs = System.Enum.GetNames(typeof(NEngine.Game.GameEnum));
        for (int i = 0; i < strs.Length; i++)
        {
            AssetVersion ver1 = AssetVersion.FromString(FileUtil.ReadFromFile(appConfigPath_old + "/Conn/" + strs[i] + "/assetVersion.config"));
            AssetbundleConfig ver2 = AssetbundleConfig.FromString(FileUtil.ReadFromFile(appConfigPath_old+ "/Conn/" + strs[i] + "/abConfig.config"));
            PreloadConfig ver3 = PreloadConfig.FromString(FileUtil.ReadFromFile(appConfigPath_old + "/Conn/" + strs[i] + "/preloadConfig.config"));
            App.GetMgr<AssetManager>().AddAssetConfig(strs[i], new AssetConfig(strs[i], ver1, ver2, ver3));
        }
    }
    /// <summary>
    /// 替换配置信息
    /// </summary>
    void replaceConfig()
    {
        FileUtil.DirCopyTo(appConfigPath_new + "/Conn", appConfigPath_old + "/Conn");
        System.IO.Directory.Delete(appConfigPath_new + "/Conn", true);
    }
    /// <summary>
    /// 根据配置信息更新
    /// </summary>
    void compareAndUpdate()
    {
        APPVersion appnew = APPVersion.FromStr(FileUtil.ReadFromFile(appConfigPath_new + "/" + appversion_config));
        APPVersion appold = getLocal();
        string[] up = appnew.GetUpdateResult(appold);
        if (up != null)
        {
            for (int i = 0; i < up.Length; i++)
                updateDll(up[i]);
        }
        updateABRes(getUpdatelist());
    }
    void updateDll(string game)
    {
        Debug.Log(string.Format("准备更新游戏{0}的dll", game));
    }
    /// <summary>
    /// 从服务器更新ab包资源
    /// </summary>
    /// <param name="list"></param>
    void updateABRes(string[] list)
    {
        if (list == null) return;
        for (int i = 0; i < list.Length; i++)
        {
            if (System.IO.File.Exists(appResPath + "/" + list[i])) System.IO.File.Delete(appResPath + "/" + list[i]);
            NEngine.Assets.AssetBundleLoader.LoadFileFromServerSync(serResAddr + "/" + list[i],_ => FileUtil.SaveFile(appResPath + "/" + list[i],_));  //下载并保存
        }
    }
    APPVersion getLocal()
    {
        if (System.IO.File.Exists(appConfigPath_old + "/" + appversion_config))
        {
            return APPVersion.FromStr(FileUtil.ReadFromFile(appConfigPath_old + "/" + appversion_config));
        }
        return APPVersion.FromStr(FileUtil.ReadFromFile(Application.streamingAssetsPath + "/appVersion.con"));
    }
}
