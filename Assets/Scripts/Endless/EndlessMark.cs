using UnityEngine;

public class EndlessMark : MonoBehaviour
{
    private int times;//用来控制对象在指定帧数内，只能碰撞一次

    public void Start()
    {
        times = 0;
    }

    public void Update()
    {
        if (times > 0)
        {
            times--;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (times > 0)
            return;
        LoadScene.Instance.InactiveScene();
        transform.position += Vector3.up * LoadScene.Instance.SpaceBetweenLoadMark;
        LoadScene.Instance.LoadNextScene();
        times = 10;
    }

}
