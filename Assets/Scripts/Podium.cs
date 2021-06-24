using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Podium : MonoBehaviour
{

    public GameController gameController;
    public Player player;
    public GameObject confetti;
    public bool levelComplete = false; 
    
    private float delay = 0.8f;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != player.gameObject.tag) return;
        levelComplete = true;
        confetti.GetComponent<Animator>().Play("Confetti_active");
        StartCoroutine(DelayedRestart());
    }

    IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(delay);
        levelComplete = false;
        gameController.resetCurrentLevel();
    }
}
