using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class DeadAnimation : MonoBehaviour
{
    public void OnEnable()
    {
        RandonVelocity();
    }

    void RandonVelocity()
    {
        int index = 0;
        foreach (Transform child in transform)
        {
            if (index % 2 == 0)
            {
                float x_A = Random.Range(1, 3);
                float y_A = Random.Range(2, 5);

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

            index++;
        }
    }
}
