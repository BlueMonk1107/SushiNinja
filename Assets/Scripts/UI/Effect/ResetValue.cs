using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResetValue : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(Wait());
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.15f);
        GetComponent<Scrollbar>().value = 1;
    }
}
