using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGen : MonoBehaviour {

    public float size = 1;

    private MeshFilter filter;


	// Use this for initialization
	void Start () {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.SetVertices(new List<Vector3>()
        {
            new Vector3(-size * 0.5f, 0, -size * 0.5f),
            new Vector3(size * 0.5f, 0, -size * 0.5f),
            new Vector3(size * 0.5f, 0, size * 0.5f),
            new Vector3(-size * 0.5f, 0, size * 0.5f)
        });

        mesh.SetTriangles(new List<int>()
        {
            3, 1, 0,
            3, 2, 1
        }, 0);

        mesh.SetNormals(new List<Vector3>()
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        });

        mesh.SetUVs(0, new List<Vector2>()
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1)
        });

        return mesh;
    }
}
