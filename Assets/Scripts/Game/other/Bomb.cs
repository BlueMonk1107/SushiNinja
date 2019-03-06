using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    private Material _myMaterial;
    private int _times;
    public Texture[] _bomb_Textures; 
	// Use this for initialization
	void Start ()
	{
	    _times = 0;
        _myMaterial = transform.GetComponent<Renderer>().material;
        InvokeRepeating("Play", 0.05f, 0.035f);
    }
    void Play()
    {
        if (_times < 14)
        {
            _myMaterial.mainTexture = _bomb_Textures[_times];
            _times++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
