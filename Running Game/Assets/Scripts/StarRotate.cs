﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotate : MonoBehaviour {

    public float speed;
	
	void Update () {
        transform.Rotate(Vector3.forward * speed);
	}
}
