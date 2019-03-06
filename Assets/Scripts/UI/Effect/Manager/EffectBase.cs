using UnityEngine;
using System.Collections;

public abstract class EffectBase : MonoBehaviour
{
    public Vector2 InPosition  { get; set; }
    public Vector2 OutPosition { get; set; }
    public abstract void In();

    public abstract void Out(GameObject outGameObject);

}
