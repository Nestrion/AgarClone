using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void StartGame()
    {
        audioManager.Play("ClickSound"); // Przykład dźwięku kliknięcia
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        audioManager.Play("ClickSound");
        Application.Quit();
    }

    // Obsługa najechania myszką na przycisk
    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.Play("HoverSound"); // Przykład dźwięku najechania
    }

    // Obsługa kliknięcia
    public void OnPointerClick(PointerEventData eventData)
    {
        audioManager.Play("ClickSound");
    }
}
