﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    private FollowTrack track;

	void Start () {
        track = GetComponentInParent<FollowTrack>();
	}
	
	void Update () {
		if (track) {
            if (track.spline) {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(track.spline.GetPoint(track.positionOnSpline + 0.025f) - transform.position), 0.05f);
            }
        }
	}
}
