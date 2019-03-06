using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GuidanceHalo : MonoBehaviour
{
    public List<Sprite> _halos;
    private Image haloImage;//光圈图片组件
    private float _time;//播放的时间
    private int _times;//播放的次数
	// Use this for initialization
	void OnEnable()
	{
	    _time = 0;
	    _times = 0;
	    haloImage = transform.GetComponent<Image>();
	    haloImage.sprite = _halos[_times];
	}
	
	// Update is called once per frame
	void Update ()
	{
        _time += 0.02f;
	    if (_time > 0.1f)
	    {
	        _time = 0;
	        _times++;
	        if (_times == 6)
	        {
	            gameObject.SetActive(false);
                return;
	        }
            haloImage.sprite = _halos[_times];
        }
	}
}
