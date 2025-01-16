using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class UI_Manager : MonoBehaviour
{
    /// <summary>
    /// The player
    /// </summary>
    public Player player;
    /// <summary>
    /// The table
    /// </summary>
    public LeaderboardTable table;

    private static UI_Manager _instance;

    /// <summary>
    /// Gets the instance of the UI_Manager.
    /// </summary>
    public static UI_Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UI_Manager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<UI_Manager>();
                    singletonObject.name = typeof(UI_Manager).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        if (player != null)
        {
            table.LocalPlayerEntryName = player.playerName;
            player.OnScoreChanged += table.UpdateEntryForLocalPlayer;
        }
    }

    /// <summary>
    /// Called when [destroy].
    /// </summary>
    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnScoreChanged -= table.UpdateEntryForLocalPlayer;
        }
    }
}
