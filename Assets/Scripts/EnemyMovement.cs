using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{


    [SerializeField]
    public Vector2 positionToMoveTo;

    [SerializeField]
    float timeToMove;

    [SerializeField]
    bool resetObjectAfterAnimation = false;

    [SerializeField]
    float timeAfterAnimationForReset = 0;

    [SerializeField]
    float resetTimeToMove = 0;

    public GameObject gameObjectToMove;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 startPosition = gameObjectToMove.transform.localPosition;
        StartCoroutine(LerpPosition(positionToMoveTo, startPosition, timeToMove));
        StartCoroutine(delayedAnimationPlay("Hit", timeToMove * .5f));
        StartCoroutine(delayedResetObjectPoition(startPosition, timeToMove * .5f + timeAfterAnimationForReset));
    }

    private void triggerAnimation(string triggerName)
    {
        Animator animator = gameObjectToMove.GetComponent<Animator>();
        animator.SetTrigger(triggerName);
    }


    private IEnumerator delayedAnimationPlay(string triggerName, float time)
    {
        yield return new WaitForSeconds(time);
        triggerAnimation(triggerName);
        Debug.Log("delay");
    }

    private IEnumerator delayedResetObjectPoition(Vector2 targetPosition, float duratuion)
    {
        yield return new WaitForSeconds(duratuion);
        StartCoroutine(LerpPosition(targetPosition, positionToMoveTo, timeToMove));

    }
    private IEnumerator LerpPosition(Vector2 targetPosition, Vector2 startPosition, float duration)
    {
        float time = 0;


        while (time < duration)
        {
            float t = time / duration;
            t = t * t * (5f + 5f * t);

            gameObjectToMove.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        gameObjectToMove.transform.localPosition = targetPosition;
    }


}
