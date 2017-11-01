using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEngine.Assets
{
    public class ResourceManager
    {
        public ResourceManager()
        {
        }
        public Object LoadAsset(string name)
        {
            //try
            //{
                Object oo = AssetUseRecorder.GetAsset(name);
                if (oo == null)
                {
                    oo = App.GetMgr<AssetManager>().LoadAssetFromBundle(name);
                    if (oo == null)
                        oo = loadFromRes(name);
                }
                return oo;
            //}
            //catch (System.Exception ex)
            //{
            //    Debug.LogError("loadasset error = " + ex);
            //    return null;
            //}
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string name) where T:UnityEngine.Object
        {
        //    try
       //     {
                T p = null;
                Object oo = LoadAsset(name);
                if (oo == null) return null;
                p = GameObject.Instantiate(oo) as T;
                return p;
            //}
            //catch (System.Exception ex)
            //{
            //    Debug.LogError("loadasset error = " + ex);
            //    return null;
            //}
        }
        Object loadFromRes(string name)
        {
            return Resources.Load(name);
        }
    }

    /// <summary>
    /// 当前游戏资源使用记录
    /// </summary>
    public static class AssetUseRecorder
    {
        private static Dictionary<string, Object> _recordObjDic = new Dictionary<string, Object>();   //记录游戏的临时资源，切换游戏时释放
        private static Dictionary<string, Object> _staticloadDic = new Dictionary<string, Object>();     //游戏运行时不释放
        public static void Add(string name, Object obj)
        {
            if (!_recordObjDic.ContainsKey(name))
                _recordObjDic.Add(name, obj);
            else
                Debug.LogWarning("repeated add Asset:"+name);
        }
        public static void AddToStatic(string name, Object obj)
        {
            if (!_staticloadDic.ContainsKey(name))
                _staticloadDic.Add(name, obj);
            else
                Debug.LogWarning("repeated add Asset:" + name);
        }
        public static Object GetAsset(string name)
        {
            Object oo = null;
            _recordObjDic.TryGetValue(name, out oo);
            if (oo == null) _staticloadDic.TryGetValue(name, out oo);
            return oo;
        }
        public static bool ExistAsset(string name)
        {
            return _recordObjDic.ContainsKey(name) || _staticloadDic.ContainsKey(name);
        }
        public static void Clear()
        {
            _recordObjDic.Clear();
        }
    }

}
