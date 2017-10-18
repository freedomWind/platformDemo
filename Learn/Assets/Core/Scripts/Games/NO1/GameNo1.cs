using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NEngine.Game;
using System;

public class GameNo1 :IGameBase
{
    public GameNo1(string name, IGameLoad loader) : base(name, loader)
    { }
    protected override string[] sceneName
    {
        get
        {
            return new string[] { "NO1" };
        }
    }
}
