using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public AudioClip Swooshing;
    public AudioClip Clang;
    public SplineCurve attachedSpline;
    [Range(0, 1)]
    public float positionOnSpline;
    private bool broken;
    private float repairTime = 30.0f;
    public float timer;
    //private System.Random rnd;

    void Start()
    {
        //rnd = new System.Random();
        positionOnSpline = attachedSpline.GetPositionOnSpline(transform.position);
        transform.position = attachedSpline.GetPoint(positionOnSpline);
        transform.LookAt(transform.position + attachedSpline.GetDirection(positionOnSpline));
        //rndTime = rnd.Next();

    }

    void Update()
    {
        if(timer > repairTime)
        {
            RepairWall();
            timer = 0;
        }
        timer += Time.deltaTime;
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
                AudioSource.PlayClipAtPoint(Swooshing, transform.position, 0.6f);
                //Debug.Log("broken");
            }
            else
            {
                if (gameObject.GetComponent<MeshFilter>().sharedMesh.name.Contains(other.gameObject.GetComponent<Posing>().GetPose()))
                {
                    other.gameObject.GetComponent<PlayerStats>().Passed();
                    AudioSource.PlayClipAtPoint(Swooshing, transform.position, 0.6f);
                }
                else
                {
                    other.gameObject.GetComponent<PlayerStats>().Failed();
                    AudioSource.PlayClipAtPoint(Clang, transform.position, 0.7f);
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

    private void RepairWall()
    {
        gameObject.GetComponent<MeshFilter>().sharedMesh =
    Resources.Load<GameObject>("Walls\\Wall").GetComponent<MeshFilter>().sharedMesh;
        broken = false;
    }
}
