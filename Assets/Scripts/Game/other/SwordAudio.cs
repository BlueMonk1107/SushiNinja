using UnityEngine;

public class SwordAudio : MonoBehaviour {

    Vector2[] _UV = {
        new Vector2(0f,0f),new Vector2(0.25f,0f),new Vector2(0.5f,0),new Vector2(0.75f,0),
        new Vector2(0f,0.5f),new Vector2(0.25f,0.5f),new Vector2(0.5f,0.5f),new Vector2(0.75f,0.5f)
    };

    public Transform _father;
    private int _times_One;//执行次数,初始状态设置成-1
    private int _times_Two;//执行次数,初始状态设置成-1
    private Renderer _light_One;
    private Renderer _light_Two;
    // Use this for initialization
    void Start ()
    {
        _times_One = -1;
        _times_Two = -1;
        _light_One = _father.GetChild(0).GetComponent<Renderer>();
        _light_Two = _father.GetChild(1).GetComponent<Renderer>();

    }

    void Update()
    {
        if (_times_One >= 0)
        {
            RightToLeft();
        }
        if (_times_Two >= 0)
        {
            LeftToRight();
            SpeedUp();
        }
    }


    public void SlashAudio()
	{
         //播放挥刀音效
        //MyAudio.PlayAudio (StaticParameter.s_Atk_Slash,false,StaticParameter.s_Atk_Slash_Volume);
	}
    public void RightToLeft()
    {
        if (HumanManager.JumpMark == JumpMark.SpeedUp)
            return;
        _times_One++;
        if (_times_One == 1)
        {
            //MyAudio.PlayAudio(StaticParameter.s_Atk_Slash, false, StaticParameter.s_Atk_Slash_Volume);
            _light_One.gameObject.SetActive(true);
            SetLight(HumanManager.WallMark);
        }
        if (_times_One < 4)
        {
            _light_One.material.mainTextureOffset = _UV[_times_One];
        }
        if (_times_One == 7||HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Jump)
        {
            _light_One.gameObject.SetActive(false);
            _times_One = -1;
        }
    }

    public void LeftToRight()
    {
        if (HumanManager.JumpMark == JumpMark.SpeedUp)
            return;
        _times_Two++;

        if (_times_Two == 1)
        {
            //MyAudio.PlayAudio(StaticParameter.s_Atk_Slash, false, StaticParameter.s_Atk_Slash_Volume);
            _light_Two.gameObject.SetActive(true);
            SetLight(HumanManager.WallMark);
        }
        if (_times_Two < 4)
        {
            _light_Two.material.mainTextureOffset = _UV[_times_Two];
        }
        if (_times_Two == 7 || HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Jump)
        {
            _light_Two.gameObject.SetActive(false);
            _times_Two = -1;
        }
    }

    public void SpeedUp()
    {
        if(HumanManager.JumpMark == JumpMark.Normal)
            return;
        _times_Two++;

        if (_times_Two == 0)
        {
            //MyAudio.PlayAudio(StaticParameter.s_Atk_Slash, false, StaticParameter.s_Atk_Slash_Volume);
            _light_Two.gameObject.SetActive(true);
            SetLight(HumanManager.WallMark);
        }
        if (_times_Two+1 < 4)
        {
            _light_Two.material.mainTextureOffset = _UV[_times_Two+1];
        }
        if (_times_Two == 7 || HumanManager.Nature.HumanManager_Script.CurrentState != HumanState.Jump)
        {
            _light_Two.gameObject.SetActive(false);
            _times_Two = -1;
        }
    }

    public void SetLight(WallMark wallMark)
    {
        if (wallMark == WallMark.Right)
        {
            _father.localPosition = Vector3.zero;
            _father.localEulerAngles = Vector3.zero;
        }
        else
        {
            _father.localPosition = new Vector3(-0.07f, 0.014f, 0.28f);
            _father.localEulerAngles = new Vector3(-1.79f, 8.22f, 356f);
        }
    }
	
}
