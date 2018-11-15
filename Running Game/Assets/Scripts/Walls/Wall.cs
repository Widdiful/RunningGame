using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Wall : MonoBehaviour
{

    public AudioClip crash;
    public AudioClip thud; 
    public SplineCurve attachedSpline;
    private string[] poses = new[] { "LD_RD", "LD_RO", "LD_RU", "LO_RD", "LO_RO", "LO_RU", "LU_RD", "LU_RO", "LU_RU" };
    [Range(0, 1)]
    public float positionOnSpline;
    public bool broken;
    private float repairTime = 40.0f;
    public float timer;
    //private System.Random rnd;
    private ParticleSystem particles;
    private AudioSource audioSource;
    public string wallPose;

    void Start()
    {
        int rnd = UnityEngine.Random.Range(0, poses.Length);
        BreakWall(poses[rnd]);
        broken = true;
        positionOnSpline = attachedSpline.GetPositionOnSpline(transform.position);
        transform.position = attachedSpline.GetPoint(positionOnSpline);
        transform.LookAt(transform.position + attachedSpline.GetDirection(positionOnSpline));
        particles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (broken) {
            if (timer > repairTime) {
                RepairWall();
                timer = 0;
            }
            timer += Time.deltaTime;
        }
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

    internal void BreakRandom()
    {
        BreakWall(poses[UnityEngine.Random.Range(0,poses.Length)]);
    }

    public void BreakWall(string pose)
    {
        wallPose = pose;
        gameObject.GetComponent<MeshFilter>().sharedMesh =
            Resources.Load<GameObject>("Walls\\Wall_" + pose).GetComponent<MeshFilter>().sharedMesh;
        if (pose == "LO_RU")
            transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
        broken = true;
    }

    public void RepairWall()
    {
        gameObject.GetComponent<MeshFilter>().sharedMesh =
    Resources.Load<GameObject>("Walls\\Wall").GetComponent<MeshFilter>().sharedMesh;
        broken = false;
    }

    public void setBroken(bool broken)
    {
        this.broken = broken;
    }
}
