using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine.Game;

public class GameFirst : IGameBase
{
    public GameFirst(string game, IGameLoad gb) : base(game, gb) { }
    protected override string[] sceneName
    {
        get
        {
            return new string[] {"sloading","lobby" };   //游戏启动加载场景
        }
    }
}
