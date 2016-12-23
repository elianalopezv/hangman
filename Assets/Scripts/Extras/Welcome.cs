using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/*
 * Class for managing Welcome elements:
 * Level selection
 * Start game
 * Instructions
 * */
public class Welcome : MonoBehaviour {

    public GameObject selectLevelPanel;
    public GameObject buttons;
    public GameObject gamePanel;
    public GameObject instructionsPanel;


    //===== Leave home and start game (Start button action)
    public void startGame()
    {
        buttons.gameObject.SetActive(false);
        selectLevelPanel.SetActive(true);
    }

    // ==== Active panel to choose and set level
   public void  chooseLevel(int level){
       gamePanel.GetComponent<Game>().setLevel(level);
       this.gameObject.SetActive(false);
       gamePanel.SetActive(true);
    }


    // ==== Reset status of gameobjects to see them as at the beginning when return from game
   public void goToInitialStates()
   {
       selectLevelPanel.SetActive(false);
       buttons.SetActive(true);

   }

    // ======  Hide buttons to see instructions
    public void  showInstructions(){
        buttons.SetActive(false);
        instructionsPanel.SetActive(true);
    }


    //======= Hide instructions and go show buttons again
    public void closeInstructions()
    {
        instructionsPanel.SetActive(false);
        buttons.SetActive(true);
        
    }
}
