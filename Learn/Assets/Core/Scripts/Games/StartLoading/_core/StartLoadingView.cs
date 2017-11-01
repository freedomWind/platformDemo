using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NEngine.Event;
using DG.Tweening;

public class StartLoadingView : MonoBehaviour {

    public Slider loadingSlider;
    public float dur;

    private void Start()
    {
        //DG.Tweening.DOTween.d
        //loadingSlider.value.(1, 3f);
    }
}
