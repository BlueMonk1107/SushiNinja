using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class LevelUpEffect : MonoBehaviour
{
    private static LevelUpEffect _instance;
    public static LevelUpEffect Instance
    {
        get
        {
            return _instance;
        }
    }
    private List<RectTransform> _tempList;
    private List<RectTransform> _levelupList;
    private List<RectTransform> _rankupList;
    private int[] _rankup_Positions;
    private List<Vector2> _rankup_Normal_Positions;
    private int times;//执行次数
    private int _index;//子物体的下标
    private Coroutine _coroutine;//携程对象

    // Use this for initialization
    void Start()
    {
        //测试代码
        //MyKeys.Gold_Value = 10000;
        //MyKeys.HeroOne_Level_Value = 0;
        _instance = this;
        _levelupList = new List<RectTransform>();
        _rankupList = new List<RectTransform>();
        _tempList = new List<RectTransform>();
        _rankup_Normal_Positions = new List<Vector2>();
        _rankup_Positions = new[] {38, -30, -100, -176, -247};

        foreach (Transform child in transform.GetChild(0))
        {
            _levelupList.Add(child.GetComponent<RectTransform>());
        }
        foreach (Transform child in transform.GetChild(1))
        {
            _rankupList.Add(child.GetComponent<RectTransform>());
            _rankup_Normal_Positions.Add(child.GetComponent<RectTransform>().anchoredPosition);
        }

    }

    public void LevelUp()
    {
        initialize();
        _tempList = _levelupList;
        transform.GetChild(0).gameObject.SetActive(true);
        Next();
    }
    //初始化数据
    void initialize()
    {
        times = 0;
        _index = 0;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        for (int i = 0; i < _rankupList.Count; i++)
        {
            _rankupList[i].anchoredPosition = _rankup_Normal_Positions[i];
        }
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }
    //执行动画
    void Next()
    {
        RectTransform rectTransform = _tempList[times];
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(rectTransform.DOLocalMoveY(rectTransform.anchoredPosition.y + (10f - 6 * _index), 0.05f).OnComplete(Next));
        mySequence.Append(rectTransform.DOLocalMoveY(rectTransform.anchoredPosition.y - (10f - 6 * _index), 0.05f));
        mySequence.Play();

        times++;
        if (times == _tempList.Count)
        {
            _coroutine = StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        transform.GetChild(_index).gameObject.SetActive(false);
        _index++;
        if (_index == 1)
        {
            MyAudio.PlayAudio(StaticParameter.s_Rankup, false, StaticParameter.s_Rankup_Volume);

            transform.GetChild(1).gameObject.SetActive(true);
            _tempList = _rankupList;
            times = 0;
            Next();
            int index = (MyKeys.GetHeroLevel(MyKeys.CurrentSelectedHero) - 1) % 5;
            transform.GetChild(1).localPosition = Vector3.right * transform.GetChild(1).localPosition.x + Vector3.up * _rankup_Positions[index];
        }
    }
}
