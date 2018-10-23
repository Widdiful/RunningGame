using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {

    private List<GameObject> roads;

    public float RoadWidth = 10;

    // Use this for initialization
    void Start () {
        roads = new List<GameObject>();        
    }

    // Update is called once per frame
    void Update () {

    }







    //Manual MeshGeneration
    //private GameObject CreateRoadPart(float width, float height)
    //{
    //    GameObject road = new GameObject();

    //    Mesh mesh = new Mesh();

    //    mesh.vertices = new Vector3[] {
    //     new Vector3(-width, -height, 0.01f),
    //     new Vector3(width, -height, 0.01f),
    //     new Vector3(width, height, 0.01f),
    //     new Vector3(-width, height, 0.01f)
    // };
    //    mesh.uv = new Vector2[] {
    //     new Vector2 (0, 0),
    //     new Vector2 (0, 1),
    //     new Vector2(1, 1),
    //     new Vector2 (1, 0)
    // };
    //    mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    //    mesh.RecalculateNormals();

    //    MeshFilter meshFilter = road.AddComponent<MeshFilter>();
    //    meshFilter.mesh = mesh;
    //    MeshRenderer meshRenderer = road.AddComponent<MeshRenderer>();
    //    meshRenderer.material.shader = Shader.Find("Particles/Additive");
    //    Texture2D texture = new Texture2D(1, 1);
    //    texture.SetPixel(0, 0, Color.red);
    //    texture.Apply();

    //    meshRenderer.material.mainTexture = texture;
    //    meshRenderer.material.color = Color.green;

    //    return road;
    //}
}
