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
