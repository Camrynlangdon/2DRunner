using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{

    public GameController gameController;
    public Player player; 
    private int delay = 2; 

    //rename to podiums 

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != player.gameObject.tag) return;
        StartCoroutine(DelayedRestart());
    }

    IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(delay);
        gameController.resetCurrentLevel();
    }
}
