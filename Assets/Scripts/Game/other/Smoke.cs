using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Smoke : MonoBehaviour {
    private Material _myMaterial;
    // Use this for initialization
    void Start () {
        _myMaterial = transform.GetComponent<Renderer>().material;
        _myMaterial.DOOffset(new Vector2(1, 0), 0.5f).SetLoops(-1).SetEase(Ease.Linear).SetId("smoke");
        DOTween.Play("smoke");
    }
	

}
