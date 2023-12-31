﻿using System;
using System.Collections;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public event Action NewFrame; 

    private float _framesPerSecond;

    private Coroutine _frameCoroutine;

    private void Awake()
    {
        _framesPerSecond = ConstValues.FramesPerSecond;
        _frameCoroutine = StartCoroutine(FrameCoroutine());
    }
    
    private IEnumerator FrameCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / _framesPerSecond);
            NewFrame?.Invoke();
        }
    }
}