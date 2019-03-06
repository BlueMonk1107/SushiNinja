using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    /// <summary>
    /// 进游戏的标志，只有刚进游戏，会出现一次logo界面
    /// </summary>
    public static bool First_In = true;
    private Image _touch_Image_Bg;//开始游戏背景图
    private Image _touch_Image;//开始游戏图片
    // Use this for initialization
    void Start ()
	{
        //Resolution.setDesignContentScale();

        _touch_Image = transform.GetChild(0).GetComponent<Image>();
        _touch_Image_Bg = transform.GetChild(1).GetComponent<Image>();


        if (!First_In)
        {
            UIManager.ActiveWindow(WindowID.WindowID_MainMenu);
            gameObject.SetActive(false);
        }

	    Flicker(0.2f,1);
	}

    void Update()
    {
        if(!WindowFade.BeginGame)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }
    public void Click()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        _touch_Image.DOKill();
        _touch_Image_Bg.DOKill();
        ClickEffect();

        MyAudio.PlayAudio(StaticParameter.s_UI_OK, false, StaticParameter.s_UI_OK_Volume);

        yield return new WaitForSeconds(0.2f);
        
        StartCoroutine(Fade());
        StartCoroutine(MainUI());
    }

    IEnumerator MainUI()
    {
        yield return new WaitForSeconds(0.5f);

        UIManager.ActiveWindow(WindowID.WindowID_MainMenu);
    }
    IEnumerator Fade()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetComponent<Image>().DOColor(Color.clear, 2f);
        transform.GetComponent<Image>().DOPlay();

        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetComponent<Image>().DOColor(Color.clear, 2f);
        transform.GetComponent<Image>().DOPlay();

        yield return new WaitForSeconds(1.5f);
        //显示登陆奖励
        transform.parent.GetChild(1).gameObject.SetActive(true);
        gameObject.SetActive(false);
        First_In = false;
    }
    //字幕闪烁
    void Flicker(float endValue,float duration)
    {
        _touch_Image.DOFade(endValue, duration).SetLoops(-1, LoopType.Yoyo);
        _touch_Image.DOPlay();

        _touch_Image_Bg.DOFade(endValue, duration).SetLoops(-1, LoopType.Yoyo);
        _touch_Image_Bg.DOPlay();
    }

    void ClickEffect()
    {
        _touch_Image.color = Color.white;
        _touch_Image.transform.DOScale(1.2f, 0.25f);
        _touch_Image.transform.DOPlay();

        _touch_Image_Bg.color = Color.white;
        _touch_Image_Bg.transform.DOScale(1.2f, 0.25f);
        _touch_Image_Bg.transform.DOPlay();
    }
}
