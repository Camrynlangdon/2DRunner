using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{


    [SerializeField]
    public Vector2 positionToMoveTo;

    [SerializeField]
    bool delayedStart = false;

    [SerializeField]
    float timeToMove;

    [SerializeField]
    bool resetObjectAfterAnimation = false;

    [SerializeField]
    float timeAfterAnimationForReset = 0;

    [SerializeField]
    float resetTimeToMove = 0;

    [SerializeField]
    string action;

    [SerializeField]
    string idle;

    [SerializeField]
    string[] randomEvent;

    public GameObject gameObjectToMove;
    private bool isRunning;
    private int timesMoved;
    private Vector2 startPosition;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isRunning)
        {



            if (timesMoved == 0)
            {
                StartCoroutine(LerpPosition(startPosition, positionToMoveTo, timeToMove));
                startPosition = gameObjectToMove.transform.localPosition;

            }




        }


    }

    private void triggerAnimation(string triggerName)
    {
        if (action == null || action == "") return;
        Animator animator = gameObjectToMove.GetComponent<Animator>();
        animator.SetTrigger(triggerName);
    }

    private void triggerAnimationLoopStop(string triggerName)
    {

        if (string.IsNullOrEmpty(idle)) return;
        Animator animator = gameObjectToMove.GetComponent<Animator>();
        animator.SetTrigger(triggerName);
    }

    private IEnumerator delayedAnimationPlay(string triggerName, float time)
    {
        yield return new WaitForSeconds(time);
        triggerAnimation(triggerName);
    }

    private IEnumerator LerpPosition(Vector2 startPosition, Vector2 targetPosition, float duration)
    {
        float time = 0;
        timesMoved++;
        isRunning = true;


        if (timesMoved == 1)
        {
            if (!delayedStart)
                triggerAnimation(action);
            else
                StartCoroutine(delayedAnimationPlay(action, timeToMove * .5f));
        }



        while (time < duration)
        {
            float t = time / duration;
            t = t * t * (1f + 10f * t);

            gameObjectToMove.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        gameObjectToMove.transform.localPosition = targetPosition;


        isRunning = false;
        resetPosition();
        if (targetPosition == this.startPosition)
        {
            timesMoved = 0;
            triggerAnimationLoopStop(idle);
        }
    }


    private void resetPosition()
    {
        if (resetObjectAfterAnimation && timesMoved == 1)
            StartCoroutine(LerpPosition(positionToMoveTo, startPosition, resetTimeToMove));
    }

}
