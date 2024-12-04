using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// The pause menu
    /// </summary>
    public GameObject pauseMenu;
    /// <summary>
    /// The is paused
    /// </summary>
    public bool isPaused;
    // Start is called before the first frame update
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    /// <summary>
    /// Goes to main menu.
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("AgarMain");
    }

    /// <summary>
    /// Quites the game.
    /// </summary>
    public void QuiteGame()
    {
        Application.Quit();
    }
}
