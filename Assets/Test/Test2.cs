using UnityEngine;
using System.Collections;

public class Test2 : MonoBehaviour
{
    private Mesh _mesh;
    public void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;

        Draw();
    }

    void Draw()
    {
        Vector3[] vertices = _mesh.vertices;
        float heightMax = 0;
        int index = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y > heightMax)
            {
                heightMax = vertices[i].y;
                index = i;
            }
            
        }
        
        vertices[index] = new Vector3(vertices[index].x, vertices[index].y+0.1f, vertices[index].z);

        _mesh.vertices = vertices;

        foreach (Vector2 vector2 in _mesh.uv)
        {
            Debug.Log(vector2);
        }
    }
}
