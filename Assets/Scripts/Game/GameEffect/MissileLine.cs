using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MissileLine : MonoBehaviour
{
    private Renderer _plane_One;
    private Renderer _plane_Two;

    public void Awake()
    {
        _plane_One = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        _plane_Two = transform.GetChild(0).GetChild(1).GetComponent<Renderer>();
    }

    public void OnEnable()
    {
        _plane_One.material.SetColor("_TintColor", Color.gray);
        _plane_One.material.SetColor("_TintColor", Color.gray);

        ChangeAlpha(HumanManager.Nature.Human.gameObject,gameObject,_plane_One,_plane_Two);
    }
    void ChangeAlpha(GameObject human, GameObject line, Renderer plane_One, Renderer plane_Two)
    {
        line.transform.GetChild(0).localScale = Vector3.right * line.transform.GetChild(0).localScale.x + Vector3.up * (transform.position.y - human.transform.position.y + 7) +
                                     Vector3.forward * line.transform.GetChild(0).localScale.z;

        Sequence sequence_One = DOTween.Sequence();
        sequence_One.Append(plane_One.material.DOColor(Color.gray, "_TintColor", 0.2f));
        sequence_One.Append(plane_One.material.DOColor(Color.clear, "_TintColor", 0.1f));

        Sequence sequence_Two = DOTween.Sequence();
        sequence_Two.Append(plane_Two.material.DOColor(Color.gray, "_TintColor", 0.2f));
        sequence_Two.Append(plane_Two.material.DOColor(Color.clear, "_TintColor", 0.1f));

        sequence_One.SetLoops(3, LoopType.Yoyo);
        sequence_Two.SetLoops(3, LoopType.Yoyo);

        sequence_One.Play();
        sequence_Two.Play();
    }
}
