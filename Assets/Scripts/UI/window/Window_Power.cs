using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Window_Power : MonoBehaviour
{
    private GameObject _shop_Copy;
    public void OnEnable()
    {
        Image temp = transform.GetChild(1).GetComponent<Image>();
        if (MyKeys.Diamond_Value >= MyKeys.BuyPhysicalPowerCost)
        {
            temp.raycastTarget = true;
            temp.color = Color.white;
        }
        else
        {
            temp.raycastTarget = false;
            temp.color = Color.gray;
        }

        if (!SceneManager.GetActiveScene().name.Equals("UI"))
        {
            _shop_Copy = GameUIManager.ShowShop();
        }

        if (MyKeys.Physical_Power_Value >= MyKeys.Power_Replay_Max)
        {
            Image buy = transform.GetChild(1).GetComponent<Image>();
            buy.color = Color.gray;
            buy.raycastTarget = false;
        }
    }


    public void OnDisable()
    {
        if (!SceneManager.GetActiveScene().name.Equals("UI"))
        {
            EffectManager.Instance.WindowEffect(false, _shop_Copy);
        }
    }
}
