using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class WallPlacement : MonoBehaviour {

    public GameObject wallPrefab;    
    public int numberOfWalls;
    private SplineCurve spline;
    private List<GameObject> walls = new List<GameObject>();
    private int maxWalls;
    private GameObject wallParent;

	void Start () {

        wallParent = new GameObject("Walls");
        wallParent.transform.SetParent(transform);

        spline = GetComponent<SplineCurve>();
        for (int i = 1; i <= numberOfWalls; i++)
        {
           
            GameObject newWall = Instantiate(wallPrefab, spline.GetPoint((float)i / (float)(numberOfWalls + 1)), Quaternion.identity, wallParent.transform);           
            newWall.GetComponent<Wall>().attachedSpline = spline;
        }
	}
}
