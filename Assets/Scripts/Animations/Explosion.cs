// Copyright (c) 2012-2024 FuryLion Group. All Rights Reserved.

using System.Collections;
using UnityEngine;
using Pool;

public abstract class Explosion : MonoBehaviour
{
    private const int Seconds = 1;

    private void OnEnable()
    {
        StartCoroutine(Release(Seconds)); 
    } 
    
    protected IEnumerator Release(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        PoolObject.Release(this);
    }
}
