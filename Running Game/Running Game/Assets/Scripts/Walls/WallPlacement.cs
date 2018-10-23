using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacement : MonoBehaviour {

    public GameObject wallPrefab;
    public int numberOfWalls;

    private SplineCurve spline;
    private List<GameObject> walls = new List<GameObject>();
    private int maxWalls;

	void Start () {
        spline = GetComponent<SplineCurve>();
        for (int i = 0; i < numberOfWalls; i++) {
            GameObject newWall = Instantiate(wallPrefab, spline.GetPoint((float)i / (float)numberOfWalls), Quaternion.identity);
            newWall.GetComponent<Wall>().attachedSpline = spline;
        }
	}
}
