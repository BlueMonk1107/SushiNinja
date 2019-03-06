using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DoublePromptEffect : MonoBehaviour
{
    private static DoublePromptEffect _instance;

    public static DoublePromptEffect Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DoublePromptEffect>();
            }
            return _instance;
        }
    }
    private RectTransform _rectTransform;
    public void ComeIn()
    {
        _rectTransform = transform.GetComponent<RectTransform>();

        _rectTransform.DOMoveX(86.25f, 0.5f);
        _rectTransform.DOPlay();
    }

    public void Back()
    {
        _rectTransform.DOMoveX(-86.75f, 0.5f);
        _rectTransform.DOPlay();
    }
}
