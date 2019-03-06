using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Destroy()
	{
		Destroy (this.gameObject);
	}
}
