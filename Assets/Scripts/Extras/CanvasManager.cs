using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * Class for managing some general aspects of the game:
 * Best Score
 * Transition between home and game panels
 * Game exit
 * 
 * Author: Eliana Lopez
 * */
public class CanvasManager : MonoBehaviour {

    public GameObject welcomePanel;    
    public GameObject gamePanel;
    public GameObject exitPanel;
    public Text currentScoreText, bestScoreText;
    private int currentScore, bestScore;
    private int currentPanel; // 1: Welcome panel, 2: Game panel 

	void Start () {

        //======= Initialize best score
        bestScore = PlayerPrefs.GetInt("BestScore");
        bestScoreText.text = "Best score: " + bestScore.ToString();
        currentPanel = 1;
	}

    void Update()
    {

        //========= Update best score
        currentScore = int.Parse(currentScoreText.text);
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            bestScoreText.text = "Best score: " + bestScore.ToString();
        }
    }


    //======== Stop the game, save score and return to home
    public void goHome(){
        gamePanel.SetActive(false);
        welcomePanel.SetActive(true);
        welcomePanel.GetComponent<Welcome>().goToInitialStates();
        PlayerPrefs.SetInt("BestScore", bestScore);
        Application.LoadLevel(0);
    }

    //======== Show panel to comfirm if user want to quit game
    public void showExitPanel()
    {
        exitPanel.SetActive(true);
        if (gamePanel.activeSelf) { gamePanel.SetActive(false); currentPanel = 2; }
        if (welcomePanel.activeSelf) { welcomePanel.SetActive(false); currentPanel = 1; }
    }

    //===== Hide panel if user do not want to quit game
    public void hideExitPanel()
    {
        exitPanel.SetActive(false);
        if (currentPanel == 1) welcomePanel.SetActive(true);
        else if (currentPanel == 2) gamePanel.SetActive(true);
    }

    //========= Save score and exit game
    public void exitGame()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        Application.Quit();
       
    }

}
