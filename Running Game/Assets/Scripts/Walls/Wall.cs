using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public AudioClip crash;
    public AudioClip thud; 
    public SplineCurve attachedSpline;
    [Range(0, 1)]
    public float positionOnSpline;
    private bool broken;
    private float repairTime = 30.0f;
    public float timer;
    //private System.Random rnd;
    private ParticleSystem particles;
    private AudioSource audioSource;

    void Start()
    {
        //rnd = new System.Random();
        positionOnSpline = attachedSpline.GetPositionOnSpline(transform.position);
        transform.position = attachedSpline.GetPoint(positionOnSpline);
        transform.LookAt(transform.position + attachedSpline.GetDirection(positionOnSpline));
        //rndTime = rnd.Next();
        particles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(timer > repairTime)
        {
            RepairWall();
            timer = 0;
        }
        timer += Time.deltaTime;

        //transform.position = attachedSpline.GetPoint(positionOnSpline);
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
                if (particles) particles.Play();
                if (audioSource) audioSource.PlayOneShot(thud, 0.9f);
            }
            else
            {
                if (gameObject.GetComponent<MeshFilter>().sharedMesh.name.Contains(other.gameObject.GetComponent<Posing>().GetPose()))
                {
                    other.gameObject.GetComponent<PlayerStats>().Passed();
                    if (particles) particles.Play();
                    if (audioSource) audioSource.PlayOneShot(crash, 0.9f);
                }
                else
                {
                    other.gameObject.GetComponent<PlayerStats>().Failed();
                    if (particles) particles.Play();
                    if (audioSource) audioSource.PlayOneShot(thud, 0.9f);
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

    public void RepairWall()
    {
        gameObject.GetComponent<MeshFilter>().sharedMesh =
    Resources.Load<GameObject>("Walls\\Wall").GetComponent<MeshFilter>().sharedMesh;
        broken = false;
    }
}
