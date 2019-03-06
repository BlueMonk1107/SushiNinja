using DG.Tweening;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _inVector2;
    private Vector2 _outVector2;
    // Use this for initialization
    void Start () {
        _rectTransform = transform.GetComponent<RectTransform>();
        _inVector2 = _rectTransform.anchoredPosition;
        //在屏幕外的坐标
        _rectTransform.anchoredPosition = _rectTransform.anchoredPosition + _rectTransform.sizeDelta.y * Vector2.up;
        _outVector2 = _rectTransform.anchoredPosition;
        //移动
        _rectTransform.DOAnchorPos(_inVector2, 0.4f).SetEase(Ease.Linear);
        _rectTransform.DOPlay();
    }

    void OnEnable()
    {

    }
}
