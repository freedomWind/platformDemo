using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Bulletting :MonoBehaviour
{ 
    [SerializeField]
    private Text _messageTxt;
    const float _duration = 16f;
    // Use this for initialization
    void Start () {
        BullettingAnimation();
    }
    
    //private void PlatNotice(PlatNotice info)
    //{
    //    if (info == null) return;
    //    _messageTxt.text = "";
    //    foreach (var item in info.Items)
    //    {
    //        _messageTxt.text += (item.CnNotify + "       ");
    //    }
    //    _messageTxt.rectTransform.DOKill();
    //    BullettingAnimation();
    //}

    void OnDestroy()
    {
        _messageTxt.rectTransform.DOKill();
    }

    //todo 距离稍后改成从UI获取应该Rect.width + text.width
    public void BullettingAnimation()
    {
        var scrollWidth = _messageTxt.preferredWidth + 430;
        var dur = (scrollWidth / 35.6f);
        _messageTxt.rectTransform.localPosition = Vector3.zero;
        _messageTxt.rectTransform.DOLocalMoveX(-scrollWidth, dur).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
