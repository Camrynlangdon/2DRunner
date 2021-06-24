using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIntelligence : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = .2f;

    [SerializeField]
    float staggerLength = .5f;

    public GameObject enemyGameObject;
    public Player player;
    public GameController gameController;
    public bool enemyIsStaggerd;


    private Coord playerPos;
    private Coord enemyPostion;
    private bool playerIsInRange;
    private bool isMoving;
    private bool hasTriggered = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != player.tag) return;
        playerIsInRange = true;

        if (!hasTriggered)
        {
            hasTriggered = true;
            moveEnemy();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerIsInRange = false;
        hasTriggered = false;
    }

    private void updatePlayerAndEnemyPos()
    {
        updateEnemyPosition();
        updatePlayerPos();
    }

    private void updatePlayerPos()
    {
        this.playerPos = player.getPlayerPosition();
    }

    private void updateEnemyPosition()
    {
        Vector2 currentEnemyPos = enemyGameObject.transform.position;
        float x = currentEnemyPos.x;
        float y = currentEnemyPos.y;
        Coord newEnemyPostion = new Coord(x, y);
        this.enemyPostion = newEnemyPostion;
    }

    public IEnumerator staggerEnemy()
    {
        enemyIsStaggerd = true;
        isMoving = false;
        yield return new WaitForSeconds(staggerLength);
        enemyIsStaggerd = false;
        moveEnemy();
    }

    private void moveEnemy()
    {
        if (gameController.gameIsFinished || enemyIsStaggerd) return;
        updatePlayerAndEnemyPos();
        if (!isMoving)
        {
            Vector2 enemyPos;
            enemyPos.x = this.enemyPostion.x;
            enemyPos.y = this.enemyPostion.y;

            Vector2 playerPos;
            playerPos.x = this.playerPos.x;
            playerPos.y = this.playerPos.y;

            StartCoroutine(LerpEnemyPosition(enemyPos, playerPos, movementSpeed));
        }

        IEnumerator LerpEnemyPosition(Vector2 startPosition, Vector2 targetPosition, float duration)
        {
            float time = 0;
            isMoving = true;

            while (time < duration)
            {
                if (enemyIsStaggerd)
                {
                    StartCoroutine(staggerEnemy());
                    yield break;
                }


                float t = time / duration;

                enemyGameObject.transform.position = Vector2.Lerp(startPosition, targetPosition, t);

                time += Time.deltaTime;
                yield return null;
            }
            enemyGameObject.transform.position = targetPosition;
            isMoving = false;
            if (playerIsInRange)
                moveEnemy();
        }


    }



}


