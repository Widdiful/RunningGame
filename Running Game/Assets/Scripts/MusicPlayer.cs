﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    static MusicPlayer instance = null;

    void Awake()
    {
        //Debug.Log("Music player Awake " + GetInstanceID());
        //if (instance != null)
        //{
        //    Destroy(gameObject);
        //    //print ("Duplicate music player self-destructing!");
        //}
        //else
        //{
        //    instance = this;
        //    GameObject.DontDestroyOnLoad(gameObject);
        //}
    }

    void Start()
    {
        //Debug.Log ("Music player Start " + GetInstanceID());

    }
}
