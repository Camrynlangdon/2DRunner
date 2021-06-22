using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{

    public GameController gameController;

    private int waitTime = 3; 



    public void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(GameResetDelay());
    }


    private IEnumerator GameResetDelay()
    {
        yield return new WaitForSeconds(waitTime);
        gameController.resetCurrentLevel();
    }

    //Debug.Log("ready to make ending");

    // EndSequence();



    private void EndSequence()
    {
       
        //start courotine before reset game
        
    }
        //gameController.resetCurrentLevel();
}
