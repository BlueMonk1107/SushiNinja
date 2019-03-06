using System;
using UnityEngine;
using System.Collections;

public class BGShip : MonoBehaviour
{
    public float MoveSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Math.Abs(MoveSpeed) > 0)
        {
            transform.Translate(-MoveSpeed, 0, -MoveSpeed);
        }
    }
}
