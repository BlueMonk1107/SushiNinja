using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class GameUILoading : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    if (GuidanceBase.GuidanceMark > 0)
	    {
	        MyKeys.Pause_Game = true;
	        StartCoroutine(Wait());
	    }
	    else
	    {
	        gameObject.SetActive(false);
	    }

	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        Image image = transform.GetComponent<Image>();
        image.DOColor(Color.clear, 0.5f).OnComplete(() =>
        {
            MyKeys.Pause_Game = false;
            gameObject.SetActive(false);
        });
        image.DOPlay();
    }
}
