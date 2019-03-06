using UnityEngine;
using System.Collections;

public class PreprocessingCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(MyTags.Wide_Wall_Collider_Layer) && !Jump._judge_Bool)
        {
            Jump._judge_Bool = true;
        }

    }
}
