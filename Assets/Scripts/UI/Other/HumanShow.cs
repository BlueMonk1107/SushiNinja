using UnityEngine;
using System.Collections;

public class HumanShow : MonoBehaviour
{
    private Animator _animator;
	// Use this for initialization
	void Start ()
	{
	    _animator = GetComponent<Animator>();
        InvokeRepeating("Show", 2,8);
	}

    void Show()
    {
        int temp = Random.Range(1, 4);
        _animator.SetInteger("index",temp);
        Invoke("Wait",0.2f);
    }

    void Wait()
    {
        _animator.SetInteger("index", 0);
    }
}
