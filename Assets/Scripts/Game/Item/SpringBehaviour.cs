using UnityEngine;
using System.Collections;

public class SpringBehaviour : MonoBehaviour
{
    public void OnDisable()
    {
        Animator ani = transform.GetComponent<Animator>();
        ani.SetBool(StaticParameter.Ani_Key_Broke, false);
        ani.SetBool(StaticParameter.Ani_Key_OnOff, false);
        ani.Play(StaticParameter.Ani_Key_Idle, 0, 0);
    }

    public void Reset()
    {
        Destroy(gameObject);
    }
}
