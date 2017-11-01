using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class GameMaking
{

    [MenuItem("Tools/InitGameEnvironment")]
    static void InitGames()
    {
        string[] names = System.Enum.GetNames(typeof(NEngine.Game.GameEnum));
        for (int i = 0; i < names.Length; i++)
        {
            InitGame(names[i]);
        }
    }
    static void InitGame(string game)
    {
        string rootPath = Application.dataPath + "/Core/Scripts/Games/" + game;
        if (!System.IO.Directory.Exists(rootPath))
        {
            System.IO.Directory.CreateDirectory(rootPath);
            System.IO.Directory.CreateDirectory(rootPath + "/_core");
            System.IO.Directory.CreateDirectory(rootPath + "/_logic");
            System.IO.Directory.CreateDirectory(rootPath + "/_model");
            System.IO.Directory.CreateDirectory(rootPath + "/other");
        }
        InitResourceFloder(game);
    }
    static void InitResourceFloder(string game)
    {
        string rootPath = Application.dataPath + "/Core/Resources/" + game;
        if (!System.IO.Directory.Exists(rootPath))
        {
            System.IO.Directory.CreateDirectory(rootPath);
        }
    }
}
