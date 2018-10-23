using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManipulator : MonoBehaviour {

    public List<GameObject> roads;
    private SplineCurve splineCurve;
    private float prefabHeight;
    public int maxRoads;

	// Use this for initialization
	void Start () {
        //roads = new List<GameObject>();
        //splineCurve = GameObject.FindGameObjectWithTag("Path").GetComponent<SplineCurve>();
        //prefabHeight = Resources.Load<GameObject>("RoadPart").GetComponent<MeshFilter>().sharedMesh.bounds.size.y;

        //maxRoads = (int)splineCurve.splineLength / (int)prefabHeight;

        //for (int i = 0; i < maxRoads; i++)
        //{
        //    GameObject tmp = Instantiate( Resources.Load<GameObject>("RoadPart"));
        //    Debug.Log((1.0f / maxRoads) * i);
        //    tmp.transform.position = splineCurve.GetPoint((1.0f / maxRoads) * i);
        //    roads.Add(tmp);            
        //}

        //Debug.Log(prefabHeight);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
