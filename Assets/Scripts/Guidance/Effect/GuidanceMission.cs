using UnityEngine;
using System.Collections;

public class GuidanceMission : MonoBehaviour
{
    private Transform _list;
    void OnEnable()
    {
        GameObject map = GameObject.FindGameObjectWithTag("Map");
        _list = map.transform.GetChild(0);
    }

    public void Update()
    {
        if (transform.name.Contains("1"))
        {
            transform.position = _list.GetChild(0).position;
        }
        else
        {
            transform.position = _list.GetChild(1).position;
        }
    }
}
