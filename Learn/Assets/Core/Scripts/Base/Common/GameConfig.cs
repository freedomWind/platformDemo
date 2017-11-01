using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine.Game;
using NEngine.Assets;

/// <summary>
/// 游戏配置
/// 资源路径
/// </summary>
public sealed class GameConfig
{
    public static bool IsNewVersion(string oddVer, string newVer)
    {
        return true;
    }
    public static bool IsNewAssets(AssetVersion _old, AssetVersion _new, out Dictionary<string, Hash128> newDic)
    {
        AssetVersion.CompareAndGetUpdateList(_old, _new, out newDic);
        return newDic.Count != 0;
    }
    public static string GetGameVersion(GameEnum gName, bool serOrLocal = true) //获取游戏版本
    {
        return "";
    }
    public static AssetVersion GetAssetVersion(GameEnum gName, bool serOrLocal = true)//获取资源版本
    {

        return null;
    }

#if UNITY_EDITOR
    public static string PreloadConPath(string gamename)
    {
        return Application.dataPath + "Editor/_AssetCon/" + gamename + "/preload.con";
    }
    public static string AssetverConPath(string gamename)
    {
        return Application.dataPath + "Editor/_AssetCon/" + gamename + "/assetVer.con";
    }
    public static string BundleConPath(string gamename)
    {
        return Application.dataPath + "Editor/_AssetCon/" + gamename + "/bundle.con";
    }
#endif
}
