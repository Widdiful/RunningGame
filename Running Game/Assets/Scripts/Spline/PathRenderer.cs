using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour {

    public int numberOfPoints;
    public GameObject tunnelPrefab;
    public GameObject pathPrefab;
    public bool buildPath;
    public bool buildTunnels;
    public float tunnelStart;
    public float tunnelEnd;
    public bool allowRealTimeUpdating;
    public float pathVerticalOffset;
    public GameObject thing;

    private SplineCurve spline;
    private LineRenderer line;
    private List<Transform> tunnelParts = new List<Transform>();
    private List<Transform> pathParts = new List<Transform>();
    private GameObject tunnelParent;
    private GameObject pathParent;

    private Vector3[] pathVertices;
    private Vector2[] pathUV;
    private int[] pathTriangles;

    private void Start() {
        spline = GetComponentInParent<SplineCurve>();
        line = GetComponent<LineRenderer>();

        //GenerateLineRenderer();
        if (buildPath) GeneratePath();
        if (buildTunnels) GenerateTunnel();
    }

    private void Update() {
        if (allowRealTimeUpdating) {
            //UpdatePath();
            //UpdateTunnel();
        }
    }

    private void GeneratePathMesh()
    {
        float width = 3;
        GameObject obj1 = new GameObject();
        obj1.transform.position = transform.position + new Vector3(-width, 0, 0);
        GameObject obj2 = new GameObject();
        obj2.transform.position = transform.position + new Vector3(width, 0, 0);

        Vector3 nextPoint = spline.GetPoint(0.1f);
        GameObject obj3 = new GameObject();
        float angle = Vector3.Angle(Vector3.forward, nextPoint + spline.GetDirection(0.1f));
        print(angle);
        obj3.transform.position = nextPoint + new Vector3(-width, 0, 0);
        obj3.transform.RotateAround(nextPoint, Vector3.up, angle);

        GameObject obj4 = new GameObject();
        obj4.transform.position = nextPoint + new Vector3(width, 0, 0);
        obj4.transform.RotateAround(nextPoint, Vector3.up, angle);



        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        };

        mesh.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2};
    }

    private void GenerateLineRenderer() {
        if (!line) {
            line = gameObject.AddComponent<LineRenderer>();
        }
        line.loop = spline.Loop;

        line.positionCount = numberOfPoints + 1;
        for (int i = 0; i < numberOfPoints; i++) {
            line.SetPosition(i, spline.GetPoint((float)i / (float)numberOfPoints));
        }
        line.SetPosition(numberOfPoints, spline.GetPoint(1));
    }

    private void GeneratePath() {
        pathParent = new GameObject("Paths");
        pathParent.transform.SetParent(transform);

        Vector3[] vertices = new Vector3[numberOfPoints * 2];
        int[] triangles = new int[numberOfPoints * 12];

        foreach (Transform path in pathParts) {
            if (path) {
                Destroy(path.gameObject);
            }
        }

        for (int i = 0; i < numberOfPoints; i++) {
            Transform newPath = Instantiate(pathPrefab, pathParent.transform).transform;
            newPath.localScale *= 0.5f;
            Vector3 position = spline.GetPoint((float)i / (float)numberOfPoints);
            position.y += pathVerticalOffset;
            Vector3 nextPosition = spline.GetPoint((float)(i + 1) / (float)numberOfPoints);
            nextPosition.y += pathVerticalOffset;
            newPath.position = position;
            newPath.LookAt(nextPosition);
            Vector3 newScale = newPath.localScale;
            float blockHeight = newPath.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * 0.5f;
            newScale.z = Vector3.Distance(newPath.position, nextPosition) /blockHeight / 4;
            newPath.localScale = newScale;
            vertices[i * 2] = newPath.transform.TransformPoint(newPath.GetComponent<MeshFilter>().sharedMesh.vertices[0]);
            vertices[(i * 2) + 1] = newPath.transform.TransformPoint(newPath.GetComponent<MeshFilter>().sharedMesh.vertices[2]);
            if (i > 0)
            {
                triangles[(i * 6) + 0] = (i * 2);
                triangles[(i * 6) + 1] = (i * 2) + 1;
                triangles[(i * 6) + 2] = (i * 2) - 2;
                triangles[(i * 6) + 3] = (i * 2) - 1;
                triangles[(i * 6) + 4] = (i * 2) - 2;
                triangles[(i * 6) + 5] = (i * 2) + 1;
            }

            pathParts.Add(newPath);
        }


        int j = numberOfPoints - 1;
        triangles[(j * 6) + 0] = 0;
        triangles[(j * 6) + 1] = 1;
        triangles[(j * 6) + 2] = (j * 2) - 2;
        triangles[(j * 6) + 3] = (j * 2) - 1;
        triangles[(j * 6) + 4] = (j * 2) - 2;
        triangles[(j * 6) + 5] = 1;

        Mesh mesh = new Mesh();
        mesh.name = gameObject.name;
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
        Destroy(pathParent);
    }

    private void GenerateTunnel() {
        tunnelParent = new GameObject("Tunnels");
        tunnelParent.transform.SetParent(transform);

        foreach (Transform tunnel in tunnelParts) {
            if (tunnel) {
                Destroy(tunnel.gameObject);
            }
        }

        for (int i = 0; i < numberOfPoints; i++) {
            if ((float)i / (float)numberOfPoints <= tunnelEnd) {
                Transform newTunnel = Instantiate(tunnelPrefab, tunnelParent.transform).transform;
                newTunnel.localScale *= 0.5f;
                Vector3 position = spline.GetPoint((float)i / (float)numberOfPoints);
                Vector3 nextPosition = spline.GetPoint((float)(i + 1) / (float)numberOfPoints);
                newTunnel.position = position;
                newTunnel.LookAt(nextPosition);
                Vector3 newScale = newTunnel.localScale;
                float blockHeight = newTunnel.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * 0.5f;
                newScale.z = Vector3.Distance(newTunnel.position, nextPosition) / blockHeight;
                newTunnel.localScale = newScale;

                tunnelParts.Add(newTunnel);
            }
        }
    }

    private void UpdatePath() {
        for (int i = 0; i < numberOfPoints; i++) {
            Transform newPath = pathParts[i];
            Vector3 position = spline.GetPoint((float)i / (float)numberOfPoints);
            position.y += pathVerticalOffset;
            Vector3 nextPosition = spline.GetPoint((float)(i + 1) / (float)numberOfPoints);
            nextPosition.y += pathVerticalOffset;
            newPath.position = position;
            newPath.LookAt(nextPosition);
            Vector3 newScale = newPath.localScale;
            float blockHeight = newPath.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * 0.5f;
            newScale.z = Vector3.Distance(newPath.position, nextPosition) / blockHeight;
            newPath.localScale = newScale;
        }
    }

    private void UpdateTunnel() {
        for (int i = 0; i < tunnelParts.Count; i++) {
            Transform newTunnel = tunnelParts[i];
            Vector3 position = spline.GetPoint((float)i / (float)numberOfPoints);
            Vector3 nextPosition = spline.GetPoint((float)(i + 1) / (float)numberOfPoints);
            newTunnel.position = position;
            newTunnel.LookAt(nextPosition);
            Vector3 newScale = newTunnel.localScale;
            float blockHeight = newTunnel.GetComponent<MeshFilter>().sharedMesh.bounds.size.z * 0.5f;
            newScale.z = Vector3.Distance(newTunnel.position, nextPosition) / blockHeight;
            newTunnel.localScale = newScale;
        }
    }
}
