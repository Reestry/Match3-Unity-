// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class BatAnimation : MonoBehaviour
{
    private Tweener _tweener;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartSingleUpDownAnimation() 
    {
        var upDuration = Random.Range(0.3f, 0.7f);
        var downDuration = Random.Range(0.3f, 0.7f);
        
        _tweener = transform.DOMoveY(transform.position.y + 0.5f, upDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _animator.Play("bat_jump");
                _tweener = transform.DOMoveY(transform.position.y - 0.5f, downDuration)
                    .SetEase(Ease.InOutSine);
            });
        _animator.Rebind();
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}

