using UnityEngine;
using System.Collections;

public class InitializeItem : MonoBehaviour
{
    private GameObject _copy_Effect;//光圈特效的副本
                                    // Use this for initialization
    //void Start()
    //{
    //    Initialize();
    //    //   GameObject effect_Mobel = (GameObject)StaticParameter.LoadObject("Other", "effect");
    //    //GameObject copy = Instantiate(effect_Mobel);
    //    //copy.transform.position = transform.position;
    //    //   copy.transform.localScale = Vector3.one*1.3f;
    //    //   copy.transform.eulerAngles = Vector3.zero;
    //    _copy_Effect = StaticParameter.ItemEffect(transform);

    //}
    public void OnEnable()
    {
        Initialize();
        _copy_Effect = StaticParameter.ItemEffect(transform);
    }
    // Update is called once per frame
    void Update()
    {
        if (MyKeys.Pause_Game)
            return;
        this.transform.Rotate(0, 2, 0, Space.World);
    }
    //道具角度的初始化
    void Initialize()
    {
        switch (transform.name)
        {
            case "item-burst":
                transform.eulerAngles = Vector3.right * (-60) + Vector3.up * (90) + Vector3.forward * (-90);
                break;
            case "item-dash":
                break;
            case "item-magnet":
                transform.eulerAngles = Vector3.right * (-90) + Vector3.up * (0) + Vector3.forward * (0);
                break;
            case "item-shield":
                transform.eulerAngles = Vector3.right * (270) + Vector3.up * (180) + Vector3.forward * (0);
                break;
        }

    }
    void OnDestroy()
    {
        Destroy(_copy_Effect);
        StaticParameter.ClearReference(ref _copy_Effect);
    }

 

    public void OnDisable()
    {
        Destroy(_copy_Effect);
        StaticParameter.ClearReference(ref _copy_Effect);
    }
}
