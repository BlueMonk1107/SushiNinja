using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DanglingWindowEffect : EffectBase
{
    private RectTransform _rectTransform;

    void OnEnable()
    {
        _rectTransform = transform.GetComponent<RectTransform>();
        _rectTransform.localScale = Vector3.one * 0.6f;
        In();
    }


    public override void In()
    {
        _rectTransform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBounce);
        _rectTransform.DOPlay();
    }

    public override void Out(GameObject outGameObject)
    {
        outGameObject.SetActive(false);
        //_rectTransform.DOScale(Vector3.one * 0.6f, 0.6f).SetEase(Ease.InBounce);
        //_rectTransform.DOPlay();
    }
}
