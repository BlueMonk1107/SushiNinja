using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HumanSwordDead : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    RandonVelocity();
    }

    void RandonVelocity()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("L"))
            {
                float x_A = Random.Range(0.5f, 2);
                float y_A = Random.Range(1, 3);

                Vector3 target_A = Vector3.right * (-x_A) + Vector3.up * (y_A);

                child.DOBlendableMoveBy(target_A, 1f).SetEase(Ease.OutExpo);

                child.DOBlendableMoveBy(Vector3.up * (-30f), 1f).SetEase(Ease.InExpo);

                child.DOLocalRotate(new Vector3(5, 4, 3), 0.4f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                child.DOPlay();
            }
            else
            {
                float x_B = Random.Range(1, 3);
                float y_B = Random.Range(2, 5);

                Vector3 target = Vector3.right * (x_B) + Vector3.up * (y_B);

                child.DOBlendableMoveBy(target, 1f).SetEase(Ease.OutExpo);

                child.DOBlendableMoveBy(Vector3.up * (-30f), 1f).SetEase(Ease.InExpo);

                child.DOLocalRotate(new Vector3(-5, -4, -3), 0.4f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                child.DOPlay();
            }
        }
    }
}
