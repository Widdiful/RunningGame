using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour {

    public int numberOfPoints;
    public GameObject tunnelPrefab;
    public GameObject pathPrefab;
    public bool buildPath;
    public bool buildTunnels;
    public float tunnelPercentage;
    public bool allowRealTimeUpdating;
    public float pathVerticalOffset;

    private SplineCurve spline;
    private LineRenderer line;
    private List<Transform> tunnelParts = new List<Transform>();
    private List<Transform> pathParts = new List<Transform>();
    private GameObject tunnelParent;
    private GameObject pathParent;

    private void Start() {
        spline = GetComponentInParent<SplineCurve>();
        line = GetComponent<LineRenderer>();

        //GenerateLineRenderer();
        if (buildPath) GeneratePath();
        if (buildTunnels) GenerateTunnel();
    }

    private void Update() {
        if (allowRealTimeUpdating) {
            UpdatePath();
            UpdateTunnel();
        }
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
            newScale.z = Vector3.Distance(newPath.position, nextPosition) /blockHeight;
            newPath.localScale = newScale;

            pathParts.Add(newPath);
        }
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
            if ((float)i / (float)numberOfPoints <= tunnelPercentage) {
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
