using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacement : MonoBehaviour {

    private List<GameObject> walls;
    private SplineCurve spline;
    private int maxWalls;

	// Use this for initialization
	void Start () {
        walls = new List<GameObject>();
        spline = GameObject.FindGameObjectWithTag("Path").GetComponent<SplineCurve>();
        
      
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
