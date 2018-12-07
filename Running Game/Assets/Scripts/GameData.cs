using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameData : MonoBehaviour {

    public List<GameObject> players;
    public Camera cam;
    private Menu menu;
    public AudioSource music;

    public static GameData gameData;

    // Use this for initialization
    private void Start ()
    {
        Scene scene = SceneManager.GetActiveScene();

        //if (gameData == null)
        //    gameData = this;
        //else if (gameData != this)
        //    Destroy(gameObject);

        //DontDestroyOnLoad(this);
        players = new List<GameObject>();
        FindCam();
        music = GetComponent<AudioSource>();
        if (scene.name == "MainScene")
        {
            menu = GameObject.Find("MainMenu").GetComponent<Menu>();
        }
    }

    private void FindCam()
    {
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menu = GameObject.Find("MainMenu").GetComponent<Menu>();
            menu.ToggleActive();
        }
	}
}
