using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class LoadEffect : MonoBehaviour
{
    private List<RectTransform> _loadList;
    private int times;//执行次数
    // Use this for initialization
    void Start () {

        _loadList = new List<RectTransform>();
        times = 0;

        foreach (Transform child in transform)
	    {
            _loadList.Add(child.GetComponent<RectTransform>()); 
	    }

        Next();
    }

    void Next()
    {
        times++;
        int i = times % 13;
        RectTransform rectTransform = _loadList[i];
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(rectTransform.DOScale(1.5f, 0.08f).OnComplete(Next));
        mySequence.Append(rectTransform.DOScale(1f, 0.08f));
        mySequence.Play();
    }
}
