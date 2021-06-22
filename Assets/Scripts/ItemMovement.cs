using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{


    [SerializeField]
    public Vector2 positionToMoveTo;

    [SerializeField]
    bool delayedStart = false;

    [SerializeField]
    float timeToMove;

    [SerializeField]
    bool linearMovement = true;

    [SerializeField]
    bool resetObjectAfterAnimation = false;

    [SerializeField]
    float timeAfterAnimationForReset = 0;

    [SerializeField]
    float resetTimeToMove = 0;

    [SerializeField]
    bool loopAction = false;

    [SerializeField]
    float timeBetweenLoops = 0;

    [SerializeField]
    float timeAfterMovementForReset = 0;

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
    private bool isPlayerTouching;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerTouching = true;
        if (!isRunning && timesMoved == 0)
        {
            StartCoroutine(LerpPosition(startPosition, positionToMoveTo, timeToMove));
            startPosition = gameObjectToMove.transform.localPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerTouching = false;
        Debug.Log("player not touching object");
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


            if (!linearMovement)
            {
                t = t * t * (1f + 10f * t);
            }


            gameObjectToMove.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        gameObjectToMove.transform.localPosition = targetPosition;


        isRunning = false;
        StartCoroutine(resetPosition());
        if (targetPosition == this.startPosition)
        {
            timesMoved = 0;
            Debug.Log("object has reset movement!" + "isplayertouching " + isPlayerTouching);
            StartCoroutine(loopMovement());
            triggerAnimationLoopStop(idle);

        }
    }


    private IEnumerator loopMovement()
    {
        if (loopAction && isPlayerTouching)
        {
            yield return new WaitForSeconds(timeBetweenLoops);
            StartCoroutine(LerpPosition(startPosition, positionToMoveTo, timeToMove));
        }
    }

    private IEnumerator resetPosition()
    {
        Debug.Log("reset Item");
        if (resetObjectAfterAnimation && timesMoved == 1)
        {

            yield return new WaitForSeconds(timeAfterMovementForReset);
            StartCoroutine(LerpPosition(positionToMoveTo, startPosition, resetTimeToMove));
        }

    }

}
