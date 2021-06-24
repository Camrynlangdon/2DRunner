using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Podium : MonoBehaviour
{

    public GameController gameController;
    public Player player;
    public GameObject confetti;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != player.gameObject.tag) return;
        confetti.GetComponent<Animator>().Play("Confetti_active");
        StartCoroutine(gameController.delayedRestart());
    }


}
