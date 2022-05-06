//BUTTON SETTINGS SCRIPT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSettings : MonoBehaviour
{
    public void ButtonClick(string setting)
    { 
        if (setting == "easy")
        {
            Settings.difficulty = Settings.Difficulties.EASY;
        }

        if (setting == "medium")
        {
            Settings.difficulty = Settings.Difficulties.MEDIUM;
        }

        if (setting == "hard")
        {
            Settings.difficulty = Settings.Difficulties.HARD;
        }

        SceneManager.LoadScene("SampleScene");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
   
    
}
