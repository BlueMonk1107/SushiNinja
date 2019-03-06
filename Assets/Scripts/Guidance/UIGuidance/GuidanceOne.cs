using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GuidanceOne : GuidanceBase {

    void Awake()
    {
    }

    // Use this for initialization
    void Start ()
    {
        Initialization(transform);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectedMission(1);
        }
    }

}
