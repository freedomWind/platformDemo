using UnityEngine;
using System.Collections.Generic;
namespace NEngine
{
    public class SceneMgr
    {
        private static Dictionary<string, ISceneState> sceneStateDic = new Dictionary<string, ISceneState>();
        public static SceneStateController mController { get; private set; }
        public SceneMgr()
        {
            initScenes();
            mController = new SceneStateController();
       //     mController.SetState(new StartScene_State(mController), "");
            Debug.Log("场景管理器初始化");
           // AppFacade.Ins.Log(BugType.log, "场景管理器初始化");
        }

        private string curSceneName = "";
        public string CurSceneName
        {
            get { return curSceneName; }
        }
        private string lastSceneName = "";
        public string LastSceneName
        {
            get { return lastSceneName; }
        }

        public int SceneCount { get { return UnityEngine.SceneManagement.SceneManager.sceneCount; } }
        public UnityEngine.SceneManagement.Scene this[int index] { get { return  UnityEngine.SceneManagement.SceneManager.GetSceneAt(index); } } 
        public void LoadScene(string loadSceneName)
        {
            if (sceneStateDic.ContainsKey(loadSceneName))
            {
                ISceneState scenestate = sceneStateDic[loadSceneName];
                if (scenestate != null)
                {
                    lastSceneName = curSceneName;
                    curSceneName = loadSceneName;
                    mController.SetState(scenestate, loadSceneName);
                }
            }
        }

        void initScenes()
        {
            //场景初始化
         //   sceneStateDic.Add("start", new StartScene_State(mController));
       //     sceneStateDic.Add("login", new LoginScene_State(mController));      
          //  sceneStateDic.Add("")
        }
        //添加到管理器
        public void RegiestScene(string sname, ISceneState scenestate)
        {
           // Debug.Log("regiestScene ="+scenestate.StateName);
            if (!sceneStateDic.ContainsKey(sname))
                sceneStateDic.Add(sname, scenestate);
        }
        //
        public void UnRegiestScene(string sname)
        {
            if (!sceneStateDic.ContainsKey(sname))
                sceneStateDic.Remove(sname);
        }
    }
}
