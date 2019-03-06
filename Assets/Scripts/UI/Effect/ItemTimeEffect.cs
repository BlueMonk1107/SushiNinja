using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemTimeEffect : MonoBehaviour
{
    private RectTransform _father;
    private Text _time_Text;
    private RectTransform purple;//紫色背景
    private float _change_Once;//每次紫色条变化的值
    private int _close_Times;//关闭函数执行的次数
    public float Time { get; set; }
    // Use this for initialization
    void Start ()
	{
        //弹出动画
        _father = transform.GetComponent<RectTransform>();
        _father.localScale = new Vector3(1,0.8f,1);
        _father.DOScaleY(1, 0.2f);
        _father.DOPlay();
        //紫色背景的动态
        purple = transform.GetChild(0).GetComponent<RectTransform>();

        //purple.DOScaleX(0, Time).OnComplete(Close);
        //purple.DOPlay();
        //文本的改变
        _time_Text = transform.GetChild(2).GetComponent<Text>();
        _change_Once = 1/(Time/0.02f);
        _close_Times = 0;
	}

    void FixedUpdate()
    {
        if(MyKeys.Pause_Game)
            return;
        Time -= 0.02f;
        if (Time > 0)
        {
            purple.localScale -= Vector3.right*_change_Once;
            _time_Text.text = string.Format("{0:n2}", Time);
        }
        else
        {
            if(_close_Times>0)
                return;
            _close_Times++;
            Close();
        }
    }

    void Close()
    {
        _father.DOScaleY(0, 0.2f).OnComplete(MyDestory);
        _father.DOPlay();
    }

    void MyDestory()
    {
        DestroyImmediate(transform.parent.gameObject,true);
    }
}
