using UnityEngine;
using System.Collections;

public class LoadBGEndless : MonoBehaviour
{
    public void OnBecameVisible()
    {
        transform.parent.position = transform.position;
    }
}
