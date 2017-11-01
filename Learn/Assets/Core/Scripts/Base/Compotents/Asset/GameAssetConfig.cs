using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NEngine.Assets
{
    /// <summary>
    /// 预加载配置文件
    /// </summary>
    public class PreloadConfig
    {
        private List<string> preLoadList;
        public PreloadConfig()
        {
            preLoadList = new List<string>();
        }
        public string[] ToArray()
        {
            try
            {
                return preLoadList.ToArray();
            }
            catch
            {
                return null;
            }
        }
        public override string ToString()
        {
            return JsonUtility.ToJson(this);//LitJson.JsonMapper.ToJson(this);
        }
        public static PreloadConfig FromString(string str)
        {
            return JsonUtility.FromJson<PreloadConfig>(str);//LitJson.JsonMapper.ToObject<PreloadConfig>(str);
        }
    }
}
