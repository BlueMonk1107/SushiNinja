using UnityEngine;
using System.Collections;

public class SelectItem : MonoBehaviour {

    void Start()
    {
        
    }

    public void AddItem(int num)
    {
        BuyTheItems.AddItems = (Items)num;
    }
}
