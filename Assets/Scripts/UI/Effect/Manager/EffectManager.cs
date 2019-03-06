using UnityEngine;
using UnityEngine.EventSystems;

public class EffectManager 
{
    private static EffectManager _instance;
    public static EffectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EffectManager();
            }
            return _instance;
        }
    }
    /// <summary>
    /// 窗口特效，第一个参数为true时显示进入效果，为false显示退出效果
    /// </summary>
    /// <param name="state_Bool"></param>
    /// <param name="UI_Object"></param>
    public void WindowEffect(bool state_Bool,GameObject UI_Object)
    {
        if (state_Bool)
        {
            //显示界面,播放,界面进入动画
            UI_Object.gameObject.SetActive(true);
        }
        else
        {
            //播放,界面退出动画,播放完隐藏界面
            if (UI_Object.GetComponentInChildren<EffectBase>() != null)
            {
                UI_Object.GetComponentInChildren<EffectBase>().Out(UI_Object);
            }
            else
            {
                UI_Object.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 升级特效
    /// </summary>
    public void LevelUpEffect()
    {
        global::LevelUpEffect.Instance.LevelUp();
    }
}
