using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SizePingPongEffect : MonoBehaviour
{
    // Use this for initialization
    void Start ()
	{
        transform.DOScale(0.8f, 1).SetLoops(-1, LoopType.Yoyo);
        transform.DOPlay();
	}
}
