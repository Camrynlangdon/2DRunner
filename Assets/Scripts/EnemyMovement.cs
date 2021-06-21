using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField]
    public Vector2 positionToMoveTo;

    [SerializeField]
    float timeToMove;

    public GameObject gameObjectToMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LerpPosition(positionToMoveTo, timeToMove));
        Debug.Log("attack player!");
    }


    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = gameObjectToMove.transform.localPosition;

        while (time < duration)
        {

            float t = time / duration;
            t = t * t * (3f - 2f * t);
            gameObjectToMove.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, t);

            time += Time.deltaTime;
            yield return null;
        }
        gameObjectToMove.transform.localPosition = targetPosition;
    }
}
