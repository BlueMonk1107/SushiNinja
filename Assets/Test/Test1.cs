using UnityEngine;
using System.Collections;

public class Test1 : MonoBehaviour {
    private int _preloadAmount;
    private int _totalCount;
    private int _preloadFrames;
    public GameObject A;

    // Use this for initialization
	void Start ()
	{
	    _preloadAmount = 11;
	    _totalCount = 0;
	    _preloadFrames = 3;
	    StartCoroutine(Wait());
	    //StartCoroutine(PreloadOverTime());
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(4);
        Instantiate(A);
    }
    private IEnumerator PreloadOverTime()
    {
        yield return null;
        

        // 获得要加载的数量
        int amount = this._preloadAmount - this._totalCount;

        if (amount <= 0)
            yield break;

        int remainder = amount % this._preloadFrames;
        int numPerFrame = amount / this._preloadFrames;//1=5/3
        

        int numThisFrame = 0;

        for (int i = 0; i < this._preloadFrames; i++)
        {
            if (i < this._preloadFrames - 1)
            {
                numThisFrame = numPerFrame;
            }
            else
            {
                numThisFrame += remainder;
            }
            Debug.Log(numThisFrame);

            for (int n = 0; n < numThisFrame; n++)
            {
                Debug.Log("   new at "+Time.frameCount);
            }

            // Safety check in case something else is making instances. 
            //   Quit early if done early
            if (this._totalCount > this._preloadAmount)
                break;

            yield return null;
        }
        
    }
}
