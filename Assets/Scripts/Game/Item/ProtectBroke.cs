using UnityEngine;
using Random = UnityEngine.Random;

public class ProtectBroke : MonoBehaviour {
    private Color _color;
    private Vector3[] _direction;
    private GameObject _particle_GameObject;
    // Use this for initialization
    void Start ()
	{
        GameObject protect_Broke_Mobel = (GameObject)StaticParameter.LoadObject("Other", "ProtectParticle");
        _particle_GameObject = Instantiate(protect_Broke_Mobel);
        _particle_GameObject.transform.position = HumanManager.Nature.Human_Mesh.bounds.center;

        _color = Color.white*0.3f;
        _direction = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshFilter>().mesh.RecalculateNormals();
            _direction[i] = transform.GetChild(i).GetComponent<MeshFilter>().mesh.normals[0] * Random.Range(0.3f, 0.8f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        _color -= Color.white * 0.01f;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).Translate(_direction[i]);
            transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_TintColor", _color);
        }
	    if (_color.a < 0)
	    {
	        Destroy(gameObject);
            Destroy(_particle_GameObject);
            StaticParameter.ClearReference(ref _particle_GameObject);
        }
    }
}
