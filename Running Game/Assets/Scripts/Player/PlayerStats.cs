using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int health;
    public float positionOnSpline;

    private FollowTrack track;

    private void Start()
    {
        track = GetComponent<FollowTrack>();
    }

    private void Update()
    {
        positionOnSpline = track.spline.GetPositionOnSpline(transform.position);
    }

    public void TakeHit()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
