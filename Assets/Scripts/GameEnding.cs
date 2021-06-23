using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{

    public GameController gameController;
    private int delay = 2; 



    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        StartCoroutine(DelayedRestart());
    }

    IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(delay);
        gameController.resetCurrentLevel();
    }
}
