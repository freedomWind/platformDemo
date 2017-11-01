using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine.Game;
using NEngine.Event;
using System;

/// <summary>
/// 启动加载场景
/// 1,进入启动页
/// 2,后台加载
/// </summary>
public class StartLoading : IGameBase
{
    public static bool isLoadingOver = false;

    public StartLoading(string game, IGameLoad gb) : base(game, gb) { }
    protected override string[] sceneName
    {
        get
        {
            return new string[] { "startLoading" };   //游戏启动加载场景
        }
    }

    /// <summary>
    ///进入启动页面
    ///游戏版本更新
    ///所有游戏资源更新
    ///预加载
    ///连接服务器
    /// </summary>
    public override void StartUp()
    {
        LoadScene(0);
        if (loader.CheckGameUpdate())  //游戏逻辑版本更新
        {
            loader.DoGameUpdate(() => { StartUp(); });
        }
        string[] games = System.Enum.GetNames(typeof(GameEnum));
        for (int i = 0; i < games.Length; i++)
        {
            IGameBase gb = App.GetMgr<GameManager>()[games[i]];
            if (gb == null) continue;
            gb.GameAssetsUpdate();
        }
        loader.PreLoadAssets();
        if (loader.Connect())
        {
            isLoadingOver = true;
        }
    }

    /// <summary>
    /// 游戏事件
    /// </summary>
    public enum gEvent
    {
        loadOver,
        
    }
}
