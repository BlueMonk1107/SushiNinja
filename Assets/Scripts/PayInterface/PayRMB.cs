using UnityEngine;
using System.Collections;

public class PayRMB
{
    private static PayRMB _instance;

    public static PayRMB Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PayRMB();
            }
            return _instance;
        }
    }

    public bool PayRMBFun(float number)
    {
        //返回值显示是否支付成功
        bool isPay = true;
        Debug.Log("支付" + number);
        return isPay;
    }
}
