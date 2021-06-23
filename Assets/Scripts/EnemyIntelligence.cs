using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntelligence : MonoBehaviour
{

    float playerMovementUpdateTime = 1;

    public GameObject Enemy;
    public Player player;
    public ItemMovement itemMovement;

    Coord enemyPostion;
    private bool playerIsInRange;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != player.tag) return;
        playerIsInRange = true;

        StartCoroutine(updatePlayerPos());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerIsInRange = false;
    }

    IEnumerator updatePlayerPos()
    {
        yield return new WaitForSeconds(playerMovementUpdateTime);
        player.getPlayerPosition();
        updateEnemyPosition();

        if (playerIsInRange)
        {
            StartCoroutine(updatePlayerPos());
            moveEnemy();
        }

    }

    private void updateEnemyPosition()
    {
        Vector2 currentplayerPosition = gameObject.transform.position;
        float x = currentplayerPosition.x;
        float y = currentplayerPosition.y;
        Coord newEnemyPostion = new Coord(x, y);
        this.enemyPostion = newEnemyPostion;
    }

    private void moveEnemy()
    {
        Vector2 enemyPOS = Enemy.gameObject.transform.position;

        Vector2 playerPOS;
        playerPOS.x = player.playerPosition.x;
        playerPOS.y = player.playerPosition.y;

        Vector2 temp;
        temp.x = 0;
        temp.y = 0;

        Debug.Log(enemyPOS.x + ", " + enemyPOS.y);

        StartCoroutine(itemMovement.LerpPosition(enemyPOS, temp, 1));
    }

}
