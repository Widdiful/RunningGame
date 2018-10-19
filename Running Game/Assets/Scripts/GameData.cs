using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public List<GameObject> players;
    public Camera cam;
	// Use this for initialization
	void Start () {
        players = new List<GameObject>();
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
