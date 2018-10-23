using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRuns : MonoBehaviour {

    Transform playerTransform;
	// Use this for initialization
	void Start () {
        playerTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
        playerTransform.position += Vector3.forward * Time.deltaTime * 10;
	}
}
