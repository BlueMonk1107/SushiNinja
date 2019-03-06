using UnityEngine;
using System.Collections;
using DG.Tweening;

//突刺障碍
public class MovingObstacle : MonoBehaviour
{
    private bool _move_Bool;
    private float _judge_Distance;
    private float _move_Distance;
	Transform _human;
	// Use this for initialization
	void Start () {
		_move_Bool = true;
        _judge_Distance = 5;

	    if (gameObject.tag == MyTags.Left_Tag)
	    {
            _move_Distance = 2.3f;
        }
	    else if(gameObject.tag == MyTags.Right_Tag)
	    {
            _move_Distance = - 2.3f;
        }
	    else
	    {
	        Debug.Log("移动障碍的标签不对");
	    }
	    
		_human = HumanManager.Nature.Human;
	}
	
	// Update is called once per frame
	void Update () {
        if(!_move_Bool)
            return;
		JudgePosition (this.transform, _human, _judge_Distance,ref _move_Bool, _move_Distance);
	}
	void JudgePosition(Transform obstacle,Transform human,float judge_Distance,ref bool move_bool, float move_Distance)
	{
		float distance = obstacle.position.y - human.position.y;
		if (distance <= judge_Distance) {
			move_bool = false;
            Move(transform, move_Distance);
        }
	}

	void Move( Transform obstacle,float move_Distance)
	{
	    obstacle.DOLocalMoveX(obstacle.transform.localPosition.x + move_Distance, 0.3f);
	    obstacle.DOPlay();
	}
}
