using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int health;
    public int laps;
    public float positionOnSpline;

    private FollowTrack track;

    private void Start()
    {
        track = GetComponent<FollowTrack>();
    }

    private void Update()
    {
        //positionOnSpline = track.spline.GetPositionOnSpline(transform.position);
    }

    public void TakeHit()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetPosition() {
        return laps + positionOnSpline;
    }

    internal void Passed()
    {
        //TODO: Play sound or do some visuals to let the player know he passed the wall
        //Debug.Log("passed");
        track.IncreaseSpeed(0.1f);
    }

    internal void Failed()
    {
        Debug.Log("failed");
        TakeHit();
        track.FailObstacle();
    }
}
