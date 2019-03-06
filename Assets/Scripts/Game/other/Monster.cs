using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private GameObject _monster_Dead;

    // Use this for initialization
    void Start()
    {
        _monster_Dead = (GameObject)StaticParameter.LoadObject("Other", "MonsterDead");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("FruitCollider"))
        {
            StartCoroutine(Wait());
        }
        else if(other.name.Contains("SkillCollider")
            &&(ItemColliction.Dash.IsRun()
               ||ItemColliction.StartDash.IsRun()
               ||ItemColliction.DeadDash.IsRun())
            ) 
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        //播放音效
        MyAudio.PlayAudio(StaticParameter.s_Attack, false, StaticParameter.s_Attack_Volume);

        //摄像机震动
        CameraShake.Is_Shake_Camera = true;

        Destroy(transform.parent.gameObject);
        GameObject temp =
            Instantiate(_monster_Dead, transform.position + Vector3.up * 0.65f, transform.rotation) as GameObject;
        Destroy(temp, 1);
    }
}
