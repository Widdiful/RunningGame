using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private GameData gameData;
    private GameObject button_panel;
	private Button button_start;
	private Button button_options;
	private Button button_credits;
	private Button button_quit;
    private bool active;

    private GameManager gm;
	
	// Use this for initialization
	void Start () {
        active = true;
        button_panel = GameObject.Find("Buttons_Panel");

        button_start = GameObject.Find("Button_Start").GetComponentInChildren<Button>();
		button_options = GameObject.Find("Button_Tutorial").GetComponentInChildren<Button>();
		button_credits = GameObject.Find("Button_Credits").GetComponentInChildren<Button>();
		button_quit = GameObject.Find("Button_Quit").GetComponentInChildren<Button>();	
		
		button_start.onClick.AddListener(StartRound);
		button_options.onClick.AddListener(ShowOptions);
		button_credits.onClick.AddListener(ShowCredtis);
		button_quit.onClick.AddListener(Quit);

        gm = FindObjectOfType<GameManager>();
	}

    internal void ToggleActive()
    {
        active = !active;
        
        button_panel.gameObject.SetActive(active);

        button_start.gameObject.SetActive(active);
        button_options.gameObject.SetActive(active);
        button_credits.gameObject.SetActive(active);
        button_quit.gameObject.SetActive(active);

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
	
	void StartRound()
	{
        if (gm) {
            if (gm.gameStarted || gm.gameStartedSolo) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return;
            }
        }

        active = false;

        button_panel.gameObject.SetActive(false);

        button_start.gameObject.SetActive(false);
        button_options.gameObject.SetActive(false);
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
    }
	
	void ShowOptions()
	{
		
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
}
