using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class Ripple : MonoBehaviour {

	// Use this for initialization
	public void Animation (int mission)
	{
	    float scale = 0;
	    if (mission == 1)
	    {
	        scale = 3;
	    }
	    else
	    {
	        scale = 2;
	    }
	    Image ripple_Image = transform.GetComponent<Image>();
	    float duration = 1f;
	    ripple_Image.DOFade(0, duration).SetLoops(-1,LoopType.Restart);
	    ripple_Image.DOPlay();
	    transform.DOScale(scale, duration).SetLoops(-1, LoopType.Restart);
	    transform.DOPlay();
	}
	
}
