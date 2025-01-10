using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator transitionAnim;
    public void PlayGame()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level");
        transitionAnim.SetTrigger("Start");
    }
}
