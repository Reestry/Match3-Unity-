// Copyright (c) 2012-2023 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GoalBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce).SetAutoKill(true);
    }

    public void SetValue(int currentScore, int targetScore)
    {
        var targetValue = (float)currentScore / targetScore;
        DOTween.To(() => _slider.value, x => _slider.value = x, targetValue, 1f);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}