using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;
using System.Reflection;
using NEngine.Assets;
using NEngine.Game;

public class App
{
    private Dictionary<string, object> managerDic;
    private GameObject _mono;
    private static App _ins;
    public static App Ins
    {
        get
        {
            if (_ins == null)
                _ins = new App();
            return _ins;
        }
    }
    public EmptyBehaviour AppMono
    {
        get
        {
            return _mono.GetComponent<EmptyBehaviour>();
        }
    }
   // public 
    private App()
    {
        managerDic = new Dictionary<string, object>();
        _mono = GameObject.Find("App");
        if (_mono == null)
        {
            _mono = new GameObject("App");     
        }
        _mono.AddCompotentIfNoExsit<EmptyBehaviour>();
        GameObject.DontDestroyOnLoad(_mono);
    }

    public void StartUp()
    {
        App.GetMgr<GameManager>().StartUp(GameEnum.StartLoading.ToString()); //启动游戏
    }
    #region 获取管理器
    public static T GetMgr<T>() where T : class
    {
        //try
        //{
            return (T)GetManager(typeof(T));
        //}
        //catch (System.Exception ex)
        //{

        //    Debug.LogError(string.Format("get manager null, manager = {0}, error =", typeof(T).Name, ex.ToString()));
        //    return null;
        //}
    }

    static object GetManager(System.Type type)
    {
        object oo = null;
        _ins.managerDic.TryGetValue(type.Name, out oo);
        if (oo == null)
        {
            oo = Assembly.GetAssembly(type).CreateInstance(type.FullName);
            _ins.managerDic.Add(type.Name, oo);
        }
        return oo;
    }
    #endregion

   
}

