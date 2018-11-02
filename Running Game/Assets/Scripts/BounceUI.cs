using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceUI : MonoBehaviour {

    private bool bounceStart;
    private float bounceSize;
    private float baseSize;

    void Start()
    {
        baseSize = transform.localScale.x;
    }
	
	void Update () {
        if (bounceStart)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(bounceSize, bounceSize, 1), 0.5f);
            if (Mathf.Abs(bounceSize - transform.localScale.x) <= 0.01f)
            {
                bounceStart = false;
            }
        }
        else if (transform.localScale.x > baseSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(baseSize, baseSize, 1), 0.5f);
            if (Mathf.Abs(baseSize - transform.localScale.x) <= 0.01f)
            {
                transform.localScale = new Vector3(baseSize, baseSize, 1);
            }
        }
	}

    public void Bounce(float size)
    {
        bounceStart = true;
        bounceSize = size;
    }
}
