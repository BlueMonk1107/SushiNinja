using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ItemColliderManager : MonoBehaviour {
	GameResource.HumanNature _nature;
	float _y;

	// Use this for initialization
	void Start () {
        _nature = HumanManager.Nature;
    }
	
	void OnTriggerEnter(Collider other) {
	    if (ItemColliction.Dash.IsRun()||ItemColliction.DeadDash.IsRun()||ItemColliction.StartDash.IsRun())
	    {
            Dash(other);
        }
	    if (ItemColliction.Double.IsRun())
	    {
            Double(other);
        }
	    if (ItemColliction.Magnet.IsRun())
	    {
	        Magnet(other);
	    }
    }

	void Dash(Collider other)//冲锋
	{
		switch (other.gameObject.layer) {
		case MyTags.Fruit_Layer:
			    FruitScript fruit = other.transform.GetComponent<FruitScript> ();
			    fruit.CutFruit ();
			    break;
		case MyTags.Obstacle_Layer:
                //播放音效
                MyAudio.PlayAudio(StaticParameter.s_Obstacle_Break, false, StaticParameter.s_Obstacle_Break_Volume);
                //Destroy (other.gameObject);
                try
		        {
                    StaticParameter.DestroyOrDespawn(other.transform);
                    GameObject _obstacle = (GameObject)StaticParameter.LoadObject("Other", "BrokenObstacle");
                    GameObject copy = Instantiate(_obstacle, other.transform.position, Quaternion.identity) as GameObject;
                    Destroy(copy, 2);

                    SkinnedMeshRenderer huamnRenderer = other.transform.GetComponent<SkinnedMeshRenderer>();
                    huamnRenderer.material.DOFade(0, 1);
                }
		        catch (Exception)
		        {
		        }

                //摄像机震动
                CameraShake.Is_Shake_Camera = true;
               
			    break;
		case MyTags.Spring_Layer:
                //播放音效
                MyAudio.PlayAudio(StaticParameter.s_Obstacle_Break, false, StaticParameter.s_Obstacle_Break_Volume);
                //摄像机震动
                CameraShake.Is_Shake_Camera = true;
                //播放破坏动画
                Animator ani = other.transform.GetComponent<Animator>();
                ani.SetBool(StaticParameter.Ani_Key_Broke, true);
                //摄像机震动
                CameraShake.Is_Shake_Camera = true;
               
			    break;
		}
	}

	void Double(Collider other)//双倍
	{
        FruitScript fruit = other.GetComponent<FruitScript>();
	    try
	    {
            fruit.Is_Double_Bool = true;
        }
	    catch (Exception)
	    {
	    }
	    
		if (other.gameObject.layer == MyTags.Fruit_Layer) {
			MeshRenderer render = null; 

			
				render = other.transform.GetComponent<MeshRenderer> ();
				if (other.transform.GetComponent<FruitScript>().DoubleMaterial) {
                    render.material = fruit.DoubleMaterial;
				} else {
					Debug.Log (other.name + "少个材质");
				}
			
				
		}
	}
	void Magnet(Collider other)//磁铁
	{
		if (other.gameObject.layer == MyTags.Fruit_Layer) {
			FruitScript fruit = other.transform.GetComponent<FruitScript> ();
            //fruit.Target=this.transform.parent.transform.GetComponentInChildren<HumanColliderManager>().transform;
		    fruit.Target = _nature.Human_Mesh;

		}
	}
}
