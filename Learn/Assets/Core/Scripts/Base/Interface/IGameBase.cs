using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using NEngine.Assets;
namespace NEngine.Game
{
    public abstract class IGameBase :IGame
    {
        protected IGameLoad loader;
        public IGameBase(string name,IGameLoad loader) : base(name) { this.loader = loader; RegiestScene(); }
        protected abstract string[] sceneName { get; }             //游戏场景
        /// <summary>
        /// 启动游戏
        /// </summary>
        public void StartUp()
        {
            if (loader.isFirstLoad)
            {
                if (loader.CheckGameUpdate())  //游戏逻辑版本更新
                {
                    loader.DoGameUpdate(() => { StartUp(); });
                }
                if (!loader.isAssetsNew)   //资源更新
                    GameAssetsUpdate();
                loader.PreLoadAssets();    //预加载
            }
            loader.Connect();
            LoadScene(0);
        }
        /// <summary>
        ///资源初始化
        /// </summary>
        /// <param name="callback"></param>
        public void GameAssetsUpdate()
        {
            if (!loader.isAssetsNew)
            {
                Debug.Log("资源初始化："+Name);
                IEnumerable<string> pullist = null;
                loader.CheckAndUpdateConfig("", null);   //检测和配置文件更新
                if (loader.CheckAssetsUpdate(out pullist))  //更新资源
                {
                    loader.PullAssets(pullist);
                }
            }
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
            Debug.Log("loadScene :"+sceneName[index]);
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
    /// <summary>
    /// 游戏启动流程
    /// </summary>
    public abstract class IGameLoad
    {
        private bool _isAssetsNew = false;
        private bool _isFirstLoad = true;
        public bool isFirstLoad { get { return _isFirstLoad; } }
        public bool isAssetsNew { get { return _isAssetsNew; } }
        public virtual bool CheckGameUpdate() //检查更新(游戏逻辑)
        {
            Debug.Log("检测版本更新");
            return false;
        }
        public virtual void DoGameUpdate(System.Action callback) //更新游戏
        {
            Debug.Log("游戏逻辑更新");
        }
        public virtual void CheckAndUpdateConfig(string url,System.Action<byte[]> loaderOver) //检测更新配置文件
        {
            Debug.Log("资源配置文件更新");
            AssetBundleLoader.LoadFileFromServerSync(url, loaderOver);
        }
        protected AssetConfig LoadLocalConfig(string game)
        {
            Debug.Log("资源配置文件loading");
            string path = Application.streamingAssetsPath + "/CONFIG/" + game;
            string[] files = System.IO.Directory.GetFiles(path);
            AssetbundleConfig bundle = null;
            AssetVersion assetversion = null;
            PreloadConfig preload = null;
            string str = "";
            string name = "";
            for (int i = 0; i < files.Length; i++)
            {
                str = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(files[i]))
                {
                    name = Util.GetFileName(files[i]);
                    str = sr.ReadToEnd();
                    //str = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(str));
                    if (name == "bundle.config")
                    {                      
                        bundle = AssetbundleConfig.FromString(str);
                    }
                    else if (name =="assetVersion.config")
                    {
                        assetversion = AssetVersion.FromString(str);
                    }
                    else if (name == "preload.config")
                    {
                        preload = PreloadConfig.FromString(str);
                    }
                    sr.Close();
                }
                
            }
            return new AssetConfig(game, assetversion, bundle, preload);
        }
        public virtual bool CheckAssetsUpdate(out IEnumerable<string> updatelist)  //游戏资源(游戏资源)
        {
            Debug.Log("检测资源更新");
            updatelist = null;
            return true;
        }
        public virtual void PullAssets(IEnumerable<string> pulllist)  //拉取资源
        {
            Debug.Log("拉取资源");
            _isAssetsNew = true;
        }
        public virtual void PreLoadAssets()   //预加载
        {
            Debug.Log("预加载");
        }
        public virtual bool Connect()  //连接
        {
            Debug.Log("服务器连接");
            _isFirstLoad = false;
            return true;
        }
    }
}
