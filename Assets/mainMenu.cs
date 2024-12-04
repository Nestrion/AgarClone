using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// 
/// </summary>
public class MainMenu : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    /// <summary>
    /// The audio manager
    /// </summary>
    private AudioManager audioManager;

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        audioManager.Play("ClickSound"); // Przykład dźwięku kliknięcia
        SceneManager.LoadSceneAsync(1);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        audioManager.Play("ClickSound");
        Application.Quit();
    }

    // Obsługa najechania myszką na przycisk
    /// <summary>
    /// Called when [pointer enter].
    /// </summary>
    /// <param name="eventData">The event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.Play("HoverSound"); // Przykład dźwięku najechania
    }

    // Obsługa kliknięcia
    /// <summary>
    /// Called when [pointer click].
    /// </summary>
    /// <param name="eventData">The event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        audioManager.Play("ClickSound");
    }
}
