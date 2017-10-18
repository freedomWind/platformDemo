using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEngine.Game
{
    public struct GameEnum
    {
        public const string No1 = "dd";
        public const string No2 = "bb";
    }
    public class GameManager
    {
        private Dictionary<string, IGameBase> _dic;
        private static IGameBase _nowRunning;
        public static IGameBase NowRunning { get { return _nowRunning; } }
        public GameManager()
        {
            _dic = new Dictionary<string, IGameBase>();
            _dic.Add(GameEnum.No1, new GameNo1(GameEnum.No1,new No1Loader()));
        }
        public void StartUp(string gameName)
        {
            if (NowRunning != null && NowRunning.Name != gameName)
                NowRunning.UnLoad();
            _dic.TryGetValue(gameName, out _nowRunning);
            NowRunning.StartUp(onStartUp);
        }
        void onStartUp(bool result)
        {
            if (!result)
                Debug.LogError(string.Format("Game: {0} start up error, error code is {1}",NowRunning.Name,0));
        }
    }
}