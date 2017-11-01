using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;

public static class Util
{
    public static bool isEditor
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
    public static string NowPlatform
    {
        get
        {
#if UNITY_EDITOR && !UNITY_ANDROID
            return "windows";
#elif UNITY_EDITOR || UNITY_ANDROID
            return "android";
#elif UNITY_IOS
            return "ios";
#endif
            return "";
        }
    }
    public static string GetFileName(string fullname)
    {
        fullname = fullname.Replace("\\", "/");
        return fullname.Substring(fullname.LastIndexOf('/') + 1);
    }
    public static string ToJson(object obj)
    {
        string str = LitJson.JsonMapper.ToJson(obj);
        str = System.Text.UTF8Encoding.UTF8.GetString(System.Text.UTF8Encoding.UTF8.GetBytes(str));
        return str;
    }
    public static T AddCompotentIfNoExsit<T>(this Transform trans) where T : MonoBehaviour
    {
        return trans.gameObject.AddCompotentIfNoExsit<T>();
    }
    public static T AddCompotentIfNoExsit<T>(this GameObject obj) where T:MonoBehaviour
    {
        T t = obj.GetComponent<T>();
        if (t == null) t = obj.AddComponent<T>();
        return t;
    }

    public static void DelayOneFrame(System.Action<object> action, object o = null)
    {
        FrameUpdateManager.Instance.StartCoroutine(delay(action, o));
    }
    static IEnumerator delay(System.Action<object> action, object o = null)
    {
        yield return null;
        action(o);
    }

}
