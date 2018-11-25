using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public List<GameObject> players;
    public Camera cam;
    private Menu menu;
    public AudioSource music;
	// Use this for initialization
	void Start () {
        players = new List<GameObject>();
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        menu = GameObject.Find("MainMenu").GetComponent<Menu>();
        music = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape)) { menu.ToggleActive(); }
	}
}
