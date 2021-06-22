using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject gameObjectToAnimate;


    public void playRandomAnimation(string[] randomAnimation)

    {
        if (randomAnimation.Length == 0) return;
        int number = 1;
        int randomNum = Random.Range(0, 10);
        if (randomNum == number)//play animation
        {
            int randomIndex = Random.Range(0, randomAnimation.Length);
            int randomAnimationTriggerDelay = Random.Range(0, 10);
            delayedAnimationPlay(randomAnimation[randomIndex], randomAnimationTriggerDelay);
        }
    }
    public void triggerAnimation(string triggerName)
    {
        if (string.IsNullOrEmpty(triggerName)) return;
        Animator animator = gameObjectToAnimate.GetComponent<Animator>();
        animator.SetTrigger(triggerName);
    }

    public void triggerAnimationLoopStop(string triggerName)
    {
        if (string.IsNullOrEmpty(triggerName)) return;
        Animator animator = gameObjectToAnimate.GetComponent<Animator>();
        animator.SetTrigger(triggerName);
    }

    public IEnumerator delayedAnimationPlay(string triggerName, float time)
    {
        yield return new WaitForSeconds(time);
        triggerAnimation(triggerName);
    }
}
