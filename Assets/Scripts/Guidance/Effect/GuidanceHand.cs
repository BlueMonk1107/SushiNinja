using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GuidanceHand : MonoBehaviour
{

    public GameObject _halo;
    private Sequence mySequence;
    // Use this for initialization
    void OnEnable()
	{
	    RectTransform rect = transform.GetComponent<RectTransform>();
        rect.localPosition = Vector3.down * 90;
        _halo.SetActive(false);

        mySequence = DOTween.Sequence();
	    mySequence.Append(rect.DOLocalMoveY(rect.localPosition.y+10f, 1f).SetLoops(1,LoopType.Yoyo).OnComplete(()=> {_halo.SetActive(true);}));
	    mySequence.SetLoops(-1);
	    mySequence.Play();
	}

    void OnDisable()
    {
        RectTransform rect = transform.GetComponent<RectTransform>();
        mySequence.Kill();
        rect.localPosition = Vector3.down * 90;
    }
}
