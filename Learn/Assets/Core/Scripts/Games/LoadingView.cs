using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LoadingView : MonoBehaviour
{
    private UnityEngine.AsyncOperation async;
    public static string loadingScene;
    public static System.Action<object> OnLoadingOver = null;
    public UnityEngine.UI.Image pImage;
    public Text progressText;
    void Start()
    {
        pImage.fillAmount = 0;
        LoadAsync(loadingScene);
     //   progressText = GameObject.Find("progressText").GetComponent<Text>();
    }
    bool isBegin;

    void LoadAsync(string name)
    {
        isBegin = true;
        async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;
        //柔和加载
        Loading(60);
    }
    void Loading(int value)
    {
        if (isBegin)
        {
            LoadingBar(value, 1.2f).OnComplete(() =>
            {
                Loading(value+(int)(value*0.2));
            });
        }
        else
        {
            LoadingBar(100, 0.4f).OnComplete(LoadOver);
        }

    }

    void LoadOver()
    {
        isBegin = false;
        async.allowSceneActivation = true;
        async = null;
        if (OnLoadingOver != null)
        {
            Util.DelayOneFrame(OnLoadingOver,loadingScene);  //等待一帧
        }
    }

    void Update()
    {
        if (!isBegin) return;
        if (async.progress <= 0.89f) //加载中
        {
            //LoadingBar((int)(100 * async.progress));
            return;
        }
        isBegin = false;
       // LoadOver();
    }

    Tweener LoadingBar(int value, float duration)
    {
        Tweener tw = pImage.DOFillAmount(value * 0.01f, duration);
        tw.OnUpdate(()=> { progressText.text = ((int)(tw.fullPosition*100)).ToString()+"%"; } );
        return tw;
    }
    //void ShowProgressText()
    //{
    //    progressText.text = 
    //}
}
