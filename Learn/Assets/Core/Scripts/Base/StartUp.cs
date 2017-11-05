using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Use this for initialization
    void Start () {

        App.Ins.StartUp();
	}
}
