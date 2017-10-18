using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using NEngine.Assets;
namespace NEngine.Game
{
    public abstract class IGameBase :IGame
    {
        private IGameLoad loader;
        public IGameBase(string name,IGameLoad loader) : base(name) { this.loader = loader; RegiestScene(); }
        protected abstract string[] sceneName { get; }             //游戏场景
        /// <summary>
        /// 启动游戏
        /// </summary>
        /// <param name="callback"></param>
        public void StartUp(System.Action<bool> callback)
        {
            if (loader.CheckGameUpdate())
            {
                loader.DoGameUpdate(()=> { StartUp(callback); });
            }
            if (loader.CheckAssetsUpdate())
            {
                loader.PullAssets(_ =>
                {
                    App.GetMgr<AssetManager>().AddAssetConfig(Name, _);
                    loader.OnAssetsUpdate();
                    StartUp(callback);
                });
            }
            loader.PreLoadAssets();
            callback(loader.Connect());
            LoadScene(0);
        }
        /// <summary>
        /// 后台资源初始化
        /// </summary>
        /// <param name="callback"></param>
        public void GameBackgroundInit(System.Action callback)
        {
            if (loader.CheckGameUpdate())
            {
                loader.DoGameUpdate(()=> { GameBackgroundInit(callback); });
            }
            if (loader.CheckAssetsUpdate())
            {
                loader.PullAssets(_ =>
                {
                    App.GetMgr<AssetManager>().AddAssetConfig(Name, _);
                    loader.OnAssetsUpdate();
                    GameBackgroundInit(callback);
                });
            }
            callback();
        }

        public void UnLoad()
        {
            //资源的卸载
            //Unity场景的卸载
            //临时数据之类的卸载
        }

        #region  场景相关
        //加载游戏内场景
        public void LoadScene(int index)
        {
            if (index < 0 || index > sceneName.Length - 1)
            {
                Debug.LogError("load scene is out of arry");
                return;
            }
            LoadScene(sceneName[index]);
        }
        //加载游戏内场景
        void LoadScene(string name)
        {
             App.GetMgr<SceneMgr>().LoadScene(name);
        }
        //场景状态注册
        void RegiestScene()
        {
            string typeName = "";
            Assembly assembly = Assembly.GetExecutingAssembly();
            object[] param = new object[1];
            param[0] = SceneMgr.mController;
            for (int i = 0; i < sceneName.Length; i++)
            {
                typeName = "Scene_" + sceneName[i];
                object oo = assembly.CreateInstance(typeName, true, BindingFlags.Default, null, param, null, null);
                if (oo == null)
                    App.GetMgr<SceneMgr>().RegiestScene(sceneName[i], new ISceneState(SceneMgr.mController));
                else
                {
                    App.GetMgr<SceneMgr>().RegiestScene(sceneName[i], (ISceneState)oo);
                }
            }
        }
        void UnRegiestScene()
        {
            for (int i = 0; i < sceneName.Length; i++)
            {
                App.GetMgr<SceneMgr>().UnRegiestScene(sceneName[i]);
            }
        }
        #endregion
    }
    public abstract class IGameLoad
    {
        public virtual bool CheckGameUpdate() //检查更新(游戏逻辑)
        {
            Debug.Log("检测版本更新");
            return false;
        }
        public virtual void DoGameUpdate(System.Action callback) //更新游戏
        {
            Debug.Log("游戏逻辑更新");
        }
        public virtual bool CheckAssetsUpdate() //游戏资源(游戏资源)
        {
            Debug.Log("检测资源更新");
            return false;
        }
        public virtual void PullAssets(System.Action<AssetConfig> callback)  //拉取资源
        {
            Debug.Log("拉取资源");
        }
        public virtual void OnAssetsUpdate()  //资源更新回调
        {
            Debug.Log("资源更新回调");
        }
        public virtual void PreLoadAssets()   //预加载
        {
            Debug.Log("预加载");
        }
        public virtual bool Connect()  //连接
        {
            Debug.Log("服务器连接");
            return true;
        }
    }
}
