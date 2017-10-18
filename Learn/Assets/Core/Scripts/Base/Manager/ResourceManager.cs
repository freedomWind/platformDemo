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
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string name) where T:UnityEngine.Object
        {
            Object oo = AssetUseRecorder.GetAsset(name);
            T p = null;
            if (oo != null)
            {
                p = GameObject.Instantiate(oo) as T;
                return p;
            }
            AssetBundle ab = App.GetMgr<AssetManager>().assetConfig.LoadAssetBundle(name);
            if (ab != null)
            {
                AssetUseRecorder.Add(ab);
                p = loadFromAB<T>(ab);
            }
            else
            {
                p = loadFromRes<T>(name);
            }
            if (p != null) AssetUseRecorder.Add(null, p);
            else Debug.LogError(string.Format("asset:{0} load error",name));
            return p;
        }
        T loadFromAB<T>(AssetBundle ab) where T : UnityEngine.Object
        {
            return null;
        }
        T loadFromRes<T>(string name) where T:UnityEngine.Object
        {
            return null;
        }
    }

    /// <summary>
    /// 当前游戏资源使用记录
    /// </summary>
    public static class AssetUseRecorder
    {
        private static List<AssetBundle> _recordABList= new List<AssetBundle>();
        private static Dictionary<string, Object> _recordObjDic = new Dictionary<string, Object>();
        public static void Add(string name, Object obj)
        {
            if (!_recordObjDic.ContainsKey(name))
                _recordObjDic.Add(name, obj);
            else
                Debug.LogWarning("repeated add Asset:"+name);
        }
        public static void Add(AssetBundle ab)
        {
            _recordABList.Add(ab);
        }
        public static Object GetAsset(string name)
        {
            Object oo = null;
            _recordObjDic.TryGetValue(name, out oo);
            return oo;
        }
        public static void Clear()
        {
            _recordObjDic.Clear();
            for (int i = 0; i < _recordABList.Count; i++)
                _recordABList[i].Unload(true);
            _recordABList.Clear();
        }
    }

}
