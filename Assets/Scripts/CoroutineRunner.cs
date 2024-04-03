// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public IEnumerator HighlightTile(Tile tile) 
    {
        if (tile == null)
            yield break;
    
        var originalColor = tile.GetComponent<Renderer>().material.color;
        var highlightColor = Color.magenta;
        var duration = 0.3f; 
        
        tile.GetComponent<Renderer>().material.DOColor(highlightColor, duration).SetAutoKill(true);

        yield return new WaitForSeconds(duration);
        
        tile.GetComponent<Renderer>().material.DOColor(originalColor, duration).SetAutoKill(true);

        yield return new WaitForSeconds(duration);
    }

}

