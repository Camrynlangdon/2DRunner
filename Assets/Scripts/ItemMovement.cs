using System.Collections;
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

    AnimationController animationController;
    public GameObject gameObjectToMove;
    public Player player;
    private bool isRunning;
    private int timesMoved;
    private Vector2 startPosition;
    private bool isPlayerTouching;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != player.tag) return;
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
    }

    public IEnumerator LerpPosition(Vector2 startPosition, Vector2 targetPosition, float duration, bool localPos = true)
    {
        float time = 0;
        timesMoved++;
        isRunning = true;


        if (timesMoved == 1)
        {
            AnimationController AnimationControllerClass = gameObject.AddComponent(typeof(AnimationController)) as AnimationController;
            animationController = AnimationControllerClass;
            animationController.gameObjectToAnimate = gameObjectToMove;
            animationController.playRandomAnimation(randomEvent);
            if (!delayedStart)
                animationController.triggerAnimation(action);
            else
                StartCoroutine(animationController.delayedAnimationPlay(action, timeToMove * .5f));
        }

        while (time < duration)
        {
            float t = time / duration;
            if (!linearMovement)
            {
                t = t * t * (1f + 10f * t);
            }

            if (localPos)
                gameObjectToMove.transform.localPosition = Vector2.Lerp(startPosition, targetPosition, t);
            else
                gameObjectToMove.transform.position = Vector2.Lerp(startPosition, targetPosition, t);

            time += Time.deltaTime;
            yield return null;
        }
        if (localPos)
            gameObjectToMove.transform.localPosition = targetPosition;
        else
            gameObjectToMove.transform.position = targetPosition;


        isRunning = false;
        StartCoroutine(resetPosition());
        if (targetPosition == this.startPosition)
        {
            timesMoved = 0;
            if (loopAction && isPlayerTouching)
                StartCoroutine(loopMovement());
            else
                animationController.triggerAnimationLoopStop(idle);
        }
    }

    private IEnumerator loopMovement()
    {
        yield return new WaitForSeconds(timeBetweenLoops);
        StartCoroutine(LerpPosition(startPosition, positionToMoveTo, timeToMove));
    }

    private IEnumerator resetPosition()
    {
        if (resetObjectAfterAnimation && timesMoved == 1 || string.IsNullOrEmpty(action) && timesMoved == 1)
        {
            yield return new WaitForSeconds(timeAfterMovementForReset);
            StartCoroutine(LerpPosition(positionToMoveTo, startPosition, resetTimeToMove));
        }
    }

}
