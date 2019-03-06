using UnityEngine;
using System.Collections;

//礼包
public class Gift : MonoBehaviour
{
    private static Transform _noviceGift;
    private static Transform _superGift;

    /// <summary>
    /// 0是新手礼包，1是超值礼包，支付成功后，设置礼包的显示状态
    /// </summary>
    public static int PaySucceedToSet
    {
        set
        {
            switch (value)
            {
                case 0:
                    IsNoviceGiftPaySucceed = 1;
                    _noviceGift.gameObject.SetActive(false);
                    break;
                case 1:
                    IsSuperGiftPaySucceed = 1;
                    _superGift.gameObject.SetActive(false);
                    break;
                default:
                    Debug.Log("礼包下标错误");
                    break;
            }
        }
    }

    private static string _novice_Key = "novice";
    private static int IsNoviceGiftPaySucceed
    {
        set
        {
            if (IsNoviceGiftPaySucceed == 0)
            {
                PlayerPrefs.SetInt(_novice_Key, value);
            }
        }
        get
        {
            return PlayerPrefs.GetInt(_novice_Key, 0);
        }
    }

    private static string _super_Key = "super";
    private static int IsSuperGiftPaySucceed
    {
        set
        {
            if (IsSuperGiftPaySucceed == 0)
            {
                PlayerPrefs.SetInt(_super_Key, value);
            }
        }
        get
        {
            return PlayerPrefs.GetInt(_super_Key, 0);
        }
    }

    public void Start()
    {
        _noviceGift = transform.GetChild(3);
        _superGift = transform.GetChild(4);
        JudgeGiftActive(_noviceGift, IsNoviceGiftPaySucceed);
        JudgeGiftActive(_superGift, IsSuperGiftPaySucceed);
    }

    //点击事件调用方法
    public void BuyGift(int index)
    {
        switch (index)
        {
            case 0:
                UIManager.ActiveWindow(WindowID.WindowID_NoviceGift);
                break;
            case 1:
                UIManager.ActiveWindow(WindowID.WindowID_SuperGift);
                break;
            default:
                Debug.Log("礼包下标错误");
                break;
        }
    }

    private void JudgeGiftActive(Transform gift,int isPaySucceed)
    {
        if (isPaySucceed > 0)
        {
            gift.gameObject.SetActive(false);
        }
        else
        {
            gift.gameObject.SetActive(true);
        }
    }
}
