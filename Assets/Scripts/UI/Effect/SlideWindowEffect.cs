using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SlideWindowEffect : EffectBase
{
    private RectTransform _rectTransform;


    void OnEnable()
    {
        _rectTransform = transform.GetComponent<RectTransform>();
        InPosition = Vector2.up * 50;
        OutPosition = Vector2.up * 800;
        //位置初始化
        _rectTransform.anchoredPosition = OutPosition;
        In();
    }
    // Use this for initialization
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
    }

    public override void In()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(_rectTransform.DOAnchorPos(InPosition, 0.3f).SetEase(Ease.OutBack));
        mySequence.Append(_rectTransform.DOAnchorPos(Vector2.zero, 0.1f).SetEase(Ease.Linear));
        mySequence.Play();
    }

    public override void Out(GameObject outGameObject)
    {
        _rectTransform.DOAnchorPos(OutPosition, 0.3f).SetEase(Ease.InBack).OnComplete(()=> { outGameObject.SetActive(false);});
        _rectTransform.DOPlay();
    }
}
