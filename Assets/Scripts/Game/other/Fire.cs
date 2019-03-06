using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Fire : MonoBehaviour
{
    private Material _myMaterial;
    private int _times;

    private Vector2[] _offset_List =
    {
        new Vector2(0, 0)   ,new Vector2(0.25f, 0),
        new Vector2(0.5f, 0),new Vector2(0.75f, 0)
    };
	// Use this for initialization
	void Start ()
	{
	    _times = 0;
	    _myMaterial = transform.GetComponent<Renderer>().material;
        InvokeRepeating("Play",0.05f,0.01f);
	}

    void Play()
    {
        _myMaterial.mainTextureOffset = _offset_List[_times];
        _times++;
        _times = _times % 4;
    }
}
