using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool gameIsFinished;


    public void resetCurrentLevel()
    {
        SceneManager.LoadScene("Main");
    }

    public IEnumerator delayedRestart()
    {
        gameIsFinished = true;
        yield return new WaitForSeconds(4);
        resetCurrentLevel();
    }


}
