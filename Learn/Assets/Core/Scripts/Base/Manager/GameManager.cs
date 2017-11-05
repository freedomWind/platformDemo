using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEngine.Game
{
    public enum GameEnum
    {
        GameFirst,
        StartLoading,

    }
    public class GameManager
    {
        private Dictionary<string, IGameBase> _dic;
        private static IGameBase _nowRunning;
        public static IGameBase NowRunning { get { return _nowRunning; } }
        public GameManager()
        {
            _dic = new Dictionary<string, IGameBase>();
            _dic.Add(GameEnum.StartLoading.ToString(), new StartLoading(GameEnum.StartLoading.ToString(), new CommonGLoader(GameEnum.StartLoading)));
            _dic.Add(GameEnum.GameFirst.ToString(), new GameFirst(GameEnum.GameFirst.ToString(), new GameFirstLoader()));
            
            // _dic.Add(GameEnum.Lobby.ToString(),new )
        }
        public IGameBase this[string name]{get{ IGameBase gb = null; _dic.TryGetValue(name, out gb); return gb; } }
        public void StartUp(string gameName)
        {
            if (NowRunning != null && NowRunning.Name != gameName)
                NowRunning.UnLoad();
            _dic.TryGetValue(gameName, out _nowRunning);
            NowRunning.StartUp();
        }
    }
}