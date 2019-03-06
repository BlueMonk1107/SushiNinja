using UnityEngine;
using System.Collections;

public class EffectUV : MonoBehaviour
{
    private Renderer _effect;
	// Use this for initialization
	void Start ()
	{
	    _effect = transform.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	_effect.material.mainTextureOffset += Vector2.down*0.01f;
	}
}
