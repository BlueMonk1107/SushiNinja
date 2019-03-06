using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-0.1f, 0, 0,Space.World);
    }
}
