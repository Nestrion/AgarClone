using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class LeaderboardEntry : MonoBehaviour
{
    /// <summary>
    /// The player name
    /// </summary>
    private string playerName;
    /// <summary>
    /// Gets or sets the name of the player.
    /// </summary>
    /// <value>
    /// The name of the player.
    /// </value>
    public string PlayerName { get; set; }

    /// <summary>
    /// The score
    /// </summary>
    private int score;
    /// <summary>
    /// Gets or sets the score.
    /// </summary>
    /// <value>
    /// The score.
    /// </value>
    public int Score { get; set; }
}
