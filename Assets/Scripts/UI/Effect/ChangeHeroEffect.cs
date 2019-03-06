using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ChangeHeroEffect : EffectBase {

    private RectTransform _rectTransform;

    void OnEnable()
    {
        _rectTransform = transform.GetComponent<RectTransform>();
        InPosition = Vector2.right*-183 + Vector2.up*_rectTransform.anchoredPosition.y;
        OutPosition = Vector2.right*-283 + Vector2.up*_rectTransform.anchoredPosition.y;
        //位置初始化
        _rectTransform.anchoredPosition = OutPosition;
        In();
    }

    public override void In()
    {
        _rectTransform.DOAnchorPos(InPosition, 0.3f).SetEase(Ease.OutBack);
        _rectTransform.DOPlay();
    }

    public override void Out(GameObject outGameObject)
    {
        _rectTransform.DOAnchorPos(OutPosition, 0.3f).SetEase(Ease.InBack).OnComplete(() => { outGameObject.SetActive(false); });
        _rectTransform.DOPlay();
    }
}
