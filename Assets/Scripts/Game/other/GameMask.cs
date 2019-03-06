using UnityEngine;

public class GameMask : MonoBehaviour
{
    private Transform _mask;
    // Use this for initialization
    void Start ()
    {
        _mask = transform.GetChild(0);
        if (MyKeys.IsBuyClearDarkness)
        {
            MyKeys.IsBuyClearDarkness = false;
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        GetHumanPositionToVector2();
    }

    void GetHumanPositionToVector2()
    {
        if(HumanManager.Nature.HumanManager_Script.CurrentState == HumanState.Dead
            || HumanManager.Nature.HumanManager_Script.CurrentState == HumanState.Stop
            || HumanManager.Nature.HumanManager_Script.CurrentState == HumanState.Slowdown)
            return;
        Vector2 human = Camera.main.WorldToScreenPoint(HumanManager.Nature.Human.position + Vector3.up * 0.5f + Vector3.right * 0.4f);
        _mask.transform.position = human;
    }
}
