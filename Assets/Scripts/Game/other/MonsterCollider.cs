using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MonsterCollider : MonoBehaviour
{
    private Transform _human;
    private Transform _monster;//辣烤怪对象
    private Transform _bullet; //子弹对象
    private bool _is_Follow;//跟随人物开关
    private WallMark _monster_Face_to;
    private Coroutine _shoot_Coroutine;//射击的携程对象
    // Use this for initialization
    void Start ()
	{
	    _is_Follow = false;
        _monster = transform.GetChild(0);
	    _bullet = _monster.GetChild(4);
        _monster.gameObject.SetActive(false);
        _bullet.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (HumanManager.Nature.HumanManager_Script.CurrentState == HumanState.Dead)
        {
            if (_shoot_Coroutine != null)
            {
                StopCoroutine(_shoot_Coroutine);
            }
            return;
        }

        if (MyKeys.Pause_Game)
            return;

	    if (_is_Follow)
	    {
	        transform.position = Vector3.right*transform.position.x + Vector3.up*_human.position.y +
	                             Vector3.forward*transform.position.z;
	        if (HumanManager.WallMark != _monster_Face_to)
	        {
	            JudgeTowards();
	        }

	    }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("HumanCollider"))
        {
            transform.GetComponent<Collider>().enabled = false;
            _human = other.transform;
            _is_Follow = true;
            JudgeTowards();
            _monster.gameObject.SetActive(true);
            _monster.DOLocalMoveY(-0.5f, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _shoot_Coroutine = StartCoroutine(Shoot());
            });
            _monster.DOPlay();
        }
    }

    //判断朝向
    void JudgeTowards()
    {
        _monster_Face_to = HumanManager.WallMark;
        switch (_monster_Face_to)
        {
            case WallMark.Left:
                _monster.eulerAngles = Vector3.up * -110;
                break;
            case WallMark.Right:
                _monster.eulerAngles = Vector3.up * 110;
                break;
        }
    }

    IEnumerator Shoot()
    {
        int temp = 0;
        yield return new WaitUntil(() =>
        {
            temp++;
            return temp == 60;
        });
        temp = 0;
        Animator ani = _monster.GetComponent<Animator>();
        ani.SetBool("Shoot",true);
        yield return new WaitUntil(() =>
        {
            temp++;
            return temp == 16;
        });
        temp = 0;
        //播放音效
        MyAudio.PlayAudio(StaticParameter.s_Shoot, false, StaticParameter.s_Shoot_Volume);
        ani.SetBool("Shoot", false);

        yield return new WaitUntil(() =>
        {
            temp++;
            return temp == 21;
        });

        HumanManager.Nature.HumanManager_Script.CurrentState = HumanState.Dead;
        _is_Follow = false;
    }
}
