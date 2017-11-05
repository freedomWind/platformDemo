using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NEngine.Game;
using NEngine.Assets;
using YZL.Compress.GZip;
using YZL.Compress.LZMA;
using YZL.Compress.UPK;

public class GameMaker :MonoBehaviour {

    private void Start()
    {
        path = Application.streamingAssetsPath + "/Conn";
    }

    /// <summary>
    /// 请使用异步压缩或者解压。
    /// </summary>
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 200, 140, 80), "生成app配置文件"))
        {
            initAppConfig();
        }

        if (GUI.Button(new Rect(Screen.width / 2 + 70, 200, 140, 80), "压缩配置文件"))
        {
            UPKFolder.PackFolder(path, path + "/appConfig.upk", null);
        }
        //if (GUI.Button(new Rect(Screen.width / 2 - 210, 320, 140, 80), "LMZA解压文件"))
        //{
        //    LZMAFile.DeCompressAsync(Application.dataPath + "/music.lzma", Application.dataPath + "/lzmamusic.mp3", ShowProgress);
        //}
        //if (GUI.Button(new Rect(Screen.width / 2 + 70, 320, 140, 80), "GZIP解压文件"))
        //{
        //    GZipFile.DeCompressAsync(Application.dataPath + "/music.gzip", Application.dataPath + "/gzipmusic.mp3", ShowProgress);
        //}
        //if (GUI.Button(new Rect(Screen.width / 2 - 210, 440, 140, 80), "UPK打包文件夹"))
        //{
        //    UPKFolder.PackFolder(Application.dataPath + "/picture", Application.dataPath + "/picture.upk", ShowProgress);
        //    // UPKFolder.PackFolderAsync(Application.dataPath + "/picture", Application.dataPath + "/picture.upk", ShowProgress);
        //}
        //if (GUI.Button(new Rect(Screen.width / 2 + 70, 440, 140, 80), "UPK解包文件夹"))
        //{
        //    UPKFolder.UnPackFolder(Application.dataPath + "/picture.upk", Application.dataPath + "/", ShowProgress);
        //    //UPKFolder.UnPackFolderAsync(Application.dataPath + "/picture.upk", Application.dataPath + "/", ShowProgress);
        //}

    }

    void ShowProgress(long all, long now)
    {
        double progress = (double)now / all;
        Debug.Log("当前进度为: " + progress);
    }
    void initAppConfig()
    {
        string[] gs = System.Enum.GetNames(typeof(GameEnum));
        APPVersion app = new APPVersion();

        //AssetConfig
        for (int i = 0; i < gs.Length; i++)
        {
            GameVersion ver = new GameVersion(gs[i], "name:" + gs[i]);
            app.Add(ver);
            AssetVersion aver = new AssetVersion();
            aver.assetVersion = 0;
            AssetbundleConfig abcon = new AssetbundleConfig();
            abcon.AddAssetBundle("test", "has001");
            PreloadConfig pre = new PreloadConfig();
            
            FileUtil.SaveFile(path + "/" + gs[i] + "/assetVersion.config", aver.ToStr());
            FileUtil.SaveFile(path + "/" + gs[i] + "/abConfig.config", abcon.ToString());
            FileUtil.SaveFile(path + "/" + gs[i] + "/preloadConfig.config", pre.ToString());
        }
        FileUtil.SaveFile(path + "/appVersion.config", app.ToString());                   //appversion      
    }

    string path = "";
}
