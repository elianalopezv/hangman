using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System;


/*
 * Class for managing the game:
 * Get the words
 * Set the words to guess
 * Verify user input
 * Generate score
 * 
 * */
public class Game : MonoBehaviour {

    private int level;//============================= 1: Easy, 2: Medium, 3:Hard
    public TextAsset easyWords; //=================== File with easy words
    public TextAsset mediumWords; //================= File with medium words
    public TextAsset hardWords; //=================== File with hard words
    private List<string> words; //=================== Selected words according to level
    public Transform wordSpace; //=================== Space to show the word
    public Transform wrongLettersSpace; // =========== Space to show wrong letters
    private char[] wordToGuess; //=================== Array with the word letters
    private int errors, hits; //===================== Amount of hits and errors
    public Hangman hangman;//======================== Hangman draw
    private HashSet<string> enteredLetters; //======= Set of letters entered by user
    public GameObject endPanel;//==================== Final panel (It shows when there are no more words available)
    public Transform scorePanel; //================== Panel to update score
    private int score; //============================ Current score
 

	//Initialization
	void Start () {
       
        loadLevel();
        loadAttempt();
        score = 0;
	
	}


	// Update is called once per frame
	void Update () {

        //======== Get the input of user 
        try
        {
            if (Input.GetKey(Input.inputString)) verifyLetter(Input.inputString);
        }
        catch { } //Avoid errors when there is no input

        if (errors == 7) StartCoroutine(loseAttempt()); //================= Lose when the hangman is complete
        if (hits == wordToGuess.Length) StartCoroutine(winAttempt()); // == Win when the word is complete

        if (words.Count == 0) endGame(); //=============================== End game when there is no more words
        
	}

    //======= Set level selected by user 
    public void setLevel(int l)
    {
        level = l;
    }


    //======== Choose file of level selected by user
    private void loadLevel()
    {
        switch (level)
        {
            case 1:
                loadFile(easyWords);
                break;
            case 2:
                loadFile(mediumWords);
                break;
            case 3:
                loadFile(hardWords);
                break;
            default:
                loadFile(easyWords);
                break;

        }
    }


    //====== Load new attempt with new word but same level
    private void  loadAttempt(){
        goToInitialStates();
        selectWord();
        setUnderlines();
        
    }


    // ===== Return to initial state in game to guess a new word
    public void goToInitialStates()
    {
        foreach (Transform letter in wordSpace)
        {
            letter.GetChild(0).gameObject.SetActive(false);
            letter.GetChild(1).gameObject.SetActive(false);
        }

        foreach (Transform letter in wrongLettersSpace) letter.gameObject.SetActive(false);

        hangman.resetHangman();

        errors = 0;
        hits = 0;
        enteredLetters = new HashSet<string>();
           
    }

    //======= Load the word of the file selected
    private void loadFile(TextAsset file)
    {
        words = new List<string>();
        string[] lines = file.text.Split('\n');
        foreach (string l in lines)
        {
            words.Add(l.Trim());

        }
    }

    //===== Select random word to guess and remove it from list of words
    private void selectWord()
    {
        int index = UnityEngine.Random.Range(0, words.Count - 1);
        //Debug.Log(words[index]);
        wordToGuess = words[index].ToCharArray();
        words.RemoveAt(index);
    }

    //====== Set the correct number of underlines according to the word to guess
    private void setUnderlines()
    {
        Transform letter;
        for (int i = 0; i < wordToGuess.Length; i++)
        {
            letter = wordSpace.GetChild(i);
            letter.GetChild(0).gameObject.SetActive(true);
        }
    }

    // ==== Verify if the letter is or not in the word and show it
    public  void verifyLetter(string letter)
    {
        if (!enteredLetters.Contains(letter)) // If letter is not repeated
       {
            Transform letterSpace;
            bool correct = false;
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                if (wordToGuess[i].ToString().Equals(letter))   // Search coincidences in the word to guess and show them
                {
                    letterSpace = wordSpace.GetChild(i).GetChild(1);
                    letterSpace.GetComponent<Text>().text = letter;
                    letterSpace.gameObject.SetActive(true);
                    correct = true;
                    hits++;
                }
            }

            if (!correct) //======================================= If the letter is not in the word
            {
                foreach (Transform wrongLetter in wrongLettersSpace) // Search space to put the wrong letter
                {

                    if (!wrongLetter.gameObject.activeSelf)
                    {
                        wrongLetter.GetComponent<Text>().text = letter;
                        wrongLetter.gameObject.SetActive(true);
                        break;
                    }
                }
                errors++;
                hangman.drawBodyPart(); //======================== Draw hangman body part
            }
            enteredLetters.Add(letter); //======================== Add letter to entered letters
        }
    }


    //==== Coroutines to wait 1 second before going to the next word
    //=== this is for having time to visualize better if user won o lost

    IEnumerator winAttempt()
    {
        hits = 0;
        errors = 0;
        hangman.drawLive();
        score += wordToGuess.Length; //=========== Increase score
        scorePanel.GetComponent<Text>().text = score.ToString();
        yield return new WaitForSeconds(1);
        loadAttempt();
    }

   
    IEnumerator loseAttempt()
    {
        hits = 0;
        errors = 0;
        hangman.drawDie();
        showAnswer();
        yield return new WaitForSeconds(1);
        loadAttempt();
    }

    // Show answer when user did not hit
    private void showAnswer()
    {
        Transform letterSpace;
       
        for (int i = 0; i < wordToGuess.Length; i++)
        {
            letterSpace = wordSpace.GetChild(i).GetChild(1);
            letterSpace.GetComponent<Text>().text = wordToGuess[i].ToString();
            letterSpace.gameObject.SetActive(true);
        }
    }

    //======= Show final message when there is not more words to play
    private void endGame()
    {
        wordSpace.gameObject.SetActive(false);
        wrongLettersSpace.gameObject.SetActive(false);
        hangman.gameObject.SetActive(false);
        endPanel.SetActive(true);
    }
}
