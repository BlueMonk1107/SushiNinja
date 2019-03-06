using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class WindowFade : MonoBehaviour
{

    public static bool BeginGame { get; private set; }
	// Use this for initialization
	void Start ()
	{
	    if (!StartGame.First_In)
	    {
            gameObject.SetActive(false);
            return;
        }
	    BeginGame = false;
        StartCoroutine(Wait());
	}

    IEnumerator Wait()
    {
        //for (int i = 0; i < 3; i++)
        //{
        //    transform.parent.GetChild(i).gameObject.SetActive(false);
        //}
        
        yield return new WaitForSeconds(1f);

        transform.GetComponent<Image>().DOColor(Color.clear, 0.5f);
        transform.GetComponent<Image>().DOPlay();

        transform.GetComponentInChildren<Text>().DOColor(Color.clear, 0.5f);
        transform.GetComponentInChildren<Text>().DOPlay();

       // transform.parent.GetChild(2).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
        BeginGame = true;
        //for (int i = 0; i < 3; i++)
        //{
        //    transform.parent.GetChild(i).gameObject.SetActive(true);
        //}
    }
	
}
