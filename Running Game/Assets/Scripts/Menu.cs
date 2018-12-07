﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private GameData gameData;
    private GameObject button_panel;
	private Button button_start;
	private Button button_highScores;
    private Button button_tutorial;
    private Button button_credits;
	private Button button_quit;
    private Button button_InfiniteSpeed;
    private Button button_HellMode;
    private bool active;

    private GameManager gm;

    //Creating placeholders for HighScores output in HighScores Scene..FJ
    public Text Player1ScoreText;



    // Use this for initialization
    void Start()
    {
        //returns the HighScores value for the HighScore scene and sets the placeholder..FJ
       // if (SceneManager.GetActiveScene())
       // {
        //    PrintScores();
        //}
        active = true;
        button_panel = GameObject.Find("Buttons_Panel");

        button_start = GameObject.Find("Button_Start").GetComponentInChildren<Button>();
        button_highScores = GameObject.Find("Button_HighScores").GetComponentInChildren<Button>();
        button_tutorial = GameObject.Find("Button_Tutorial").GetComponentInChildren<Button>();
        button_credits = GameObject.Find("Button_Credits").GetComponentInChildren<Button>();
        button_quit = GameObject.Find("Button_Quit").GetComponentInChildren<Button>();
        button_InfiniteSpeed = GameObject.Find("Button_InfiniteSpeed").GetComponentInChildren<Button>();
        button_HellMode = GameObject.Find("Button_HellMode").GetComponentInChildren<Button>();

        button_start.onClick.AddListener(StartRound);
        button_highScores.onClick.AddListener(ShowHighScores);
        button_tutorial.onClick.AddListener(StartTutorial);
        button_credits.onClick.AddListener(ShowCredtis);
        button_quit.onClick.AddListener(Quit);
        button_InfiniteSpeed.onClick.AddListener(InfiniteSpeed);
        button_HellMode.onClick.AddListener(HellMode);

        gm = FindObjectOfType<GameManager>();


    }

    private void PrintScores()
    {
        Player1ScoreText.text = "x" + PlayerPrefs.GetInt("NP1SText");
    }

    internal void ToggleActive()
    {
        active = !active;
        
        button_panel.gameObject.SetActive(active);

        button_start.gameObject.SetActive(active);
        button_highScores.gameObject.SetActive(active);
        button_tutorial.gameObject.SetActive(active);
        button_credits.gameObject.SetActive(active);
        button_quit.gameObject.SetActive(active);
        button_InfiniteSpeed.gameObject.SetActive(active);
        button_HellMode.gameObject.SetActive(active);

        if (gm) {
            if (gm.gameStarted || gm.gameStartedSolo) {
                button_start.GetComponentInChildren<Text>().text = "Return to menu";
                switch (active) {
                    case true:
                        Time.timeScale = 0;
                        break;
                    case false:
                        Time.timeScale = 1;
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
	
	public void StartRound()
	{
        if (gm) {
            if (gm.gameStarted || gm.gameStartedSolo) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                return;
            }
        }

        active = false;

        button_panel.gameObject.SetActive(false);

        button_start.gameObject.SetActive(false);
        button_highScores.gameObject.SetActive(false);
        button_credits.gameObject.SetActive(false);
        button_quit.gameObject.SetActive(false);


        GameObject.Find("GameUpdates").GetComponent<DropInRound>().Playerselect = true;

        foreach (FollowTrack runner in FindObjectsOfType<FollowTrack>()) {
            Destroy(runner.gameObject);
        }

        SpectatorCamera spectator = FindObjectOfType<SpectatorCamera>();

        spectator.transform.position = new Vector3(0, 10, -10);
        spectator.transform.rotation = Quaternion.Euler(30, 0, 0);
        spectator.enabled = false;
        GameObject.Find("JoinScreenText").GetComponent<Text>().enabled = true;
    }
	
	public void ShowHighScores()
	{
        SceneManager.LoadScene(2);
    }

    public void StartTutorial() {
        SceneManager.LoadScene(1);
    }
	
	void ShowCredtis()
	{
		
	}
	
	void Quit()
	{
        Application.Quit();
	}

    public void InfiniteSpeed()
    {
        SceneManager.LoadScene(4);
    }

    public void HellMode()
    {
        SceneManager.LoadScene(3);
    }
}
