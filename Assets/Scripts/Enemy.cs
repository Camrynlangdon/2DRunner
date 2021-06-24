using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    int meleeDamage;

    [SerializeField]
    float attackSpeedInSeconds = 0;

    public Player player;
    public EnemyIntelligence enemyIntelligence;

    private bool isPlayerTouching;
    private bool alreadyAttacked;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == player.tag)
        {
            isPlayerTouching = true;

            if (enemyIntelligence != null)
                enemyIntelligence.enemyIsStaggerd = true;

            StartCoroutine(waitForAttack());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerTouching = false;
    }

    IEnumerator waitForAttack()
    {
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            player.ChangeHealth(meleeDamage);
            yield return new WaitForSeconds(attackSpeedInSeconds);

            alreadyAttacked = false;

            if (isPlayerTouching)
                StartCoroutine(waitForAttack());
        }

    }


}
