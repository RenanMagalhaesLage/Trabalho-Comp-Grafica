using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Events : MonoBehaviour
{
    public void ReplayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
       //Application.Quit();
       #if UNITY_STANDALONE
        Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
