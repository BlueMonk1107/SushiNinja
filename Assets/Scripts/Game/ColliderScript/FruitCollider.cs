using UnityEngine;
using System.Collections;

public class FruitCollider : MonoBehaviour {

	GameResource.HumanNature _nature;
	HumanManager _human;

	float _time ;
	bool _time_Bool;

    private static FruitCollider _instance;

    public static FruitCollider Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FruitCollider>();
            }
            return _instance;
        }
    }

    private Vector3 _default_Position;
    private Vector3 _default_Rotation;


    void Start () {
        _nature = HumanManager.Nature;
        _human = _nature.HumanManager_Script;
		_time = 0;
		_time_Bool = false;
        _default_Position = transform.localPosition;
        _default_Rotation = transform.localEulerAngles;
    }

	void Update ()
	{
        if (_time_Bool 
            &&(_human.CurrentState.Equals(HumanState.Run)
              || _human.CurrentState.Equals(HumanState.Revise)
              || _human.CurrentState.Equals(HumanState.Slowdown)))
        {
            _time += Time.deltaTime;
            if (_time > 0.3f)
            {
                _nature.Human_Ani.SetInteger(StaticParameter.Ani_Key_HumanState, 0);
                _time_Bool = false;
            }
        }
    }

    public void ChangeColliderRotation()
    {
        transform.Rotate(-90, 0, 0);
        transform.Translate(-0.4f, 0.6f, -1f, Space.Self);
    }

    public void ChangeRotationToDefault()
    {
        transform.localPosition = _default_Position;
        transform.localEulerAngles = _default_Rotation;
    }

}
