using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public SplineCurve attachedSpline;
    [Range(0, 1)]
    public float positionOnSpline;
    private bool broken;

    void Start()
    {
        positionOnSpline = attachedSpline.GetPositionOnSpline(transform.position);
        transform.position = attachedSpline.GetPoint(positionOnSpline);
        transform.LookAt(transform.position + attachedSpline.GetDirection(positionOnSpline));
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("broken");
        if (other.gameObject.CompareTag("Player"))
        {
            if (!broken)
            {
                BreakWall(other.gameObject.GetComponent<Posing>().GetPose());
                other.gameObject.GetComponent<PlayerStats>().Passed();
                //Debug.Log("broken");
            }
            else
            {
                if (gameObject.GetComponent<MeshFilter>().sharedMesh.name.Contains(other.gameObject.GetComponent<Posing>().GetPose()))
                {
                    other.gameObject.GetComponent<PlayerStats>().Passed();
                }
                else
                {
                    other.gameObject.GetComponent<PlayerStats>().Failed();
                }
            }
        }

    }

    private void BreakWall(string pose)
    {
       // Debug.Log("Wall_" + pose);
        gameObject.GetComponent<MeshFilter>().sharedMesh =
            Resources.Load<GameObject>("Walls\\Wall_" + pose).GetComponent<MeshFilter>().sharedMesh;
        broken = true;
    }
}
