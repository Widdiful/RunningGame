using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCamera : MonoBehaviour {
    public float changeTime;
    public float lerpSpeed;
    private float changeTimer;
    private Transform target;

    private void Update()
    {
        changeTimer -= Time.fixedDeltaTime;
        if (changeTimer <= 0)
        {
            changeTimer = changeTime;
            target = FindObjectsOfType<FollowTrack>()[Random.Range(0, FindObjectsOfType<FollowTrack>().Length)].transform;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), lerpSpeed);
    }
}
