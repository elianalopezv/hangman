using UnityEngine;
using System.Collections;


/*
 * Class for managing the hangman draw
 * 
 * */
public class Hangman : MonoBehaviour {

    public Transform body;
    public Transform dieSignal, liveSignal;
	


    //========== Draw the next part of body. This is called when user type a wrong letter
    public void drawBodyPart()
    {

        foreach (Transform part in body)
        {

            if (!part.gameObject.activeSelf)
            {
                part.gameObject.SetActive(true);
                break;
            }
        }
    }

    // === Draw a a green signal indicating success
    public void drawLive()
    {
        liveSignal.gameObject.SetActive(true);
        liveSignal.GetComponent<AudioSource>().Play();
    }

    //==== Draw red signal indicating failure 
    public void drawDie()
    {
        dieSignal.gameObject.SetActive(true);
        dieSignal.GetComponent<AudioSource>().Play();
    }


    // Reset the draw for initialize new attempt
    public void resetHangman()
    {
        liveSignal.gameObject.SetActive(false);
        dieSignal.gameObject.SetActive(false);
        foreach (Transform part in body) part.gameObject.SetActive(false);
    }
}
