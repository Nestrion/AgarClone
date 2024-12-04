using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// The player score
    /// </summary>
    private int playerScore = 0;
    /// <summary>
    /// The player name
    /// </summary>
    public string playerName = "placeholder";
    /// <summary>
    /// The player mass
    /// </summary>
    private float playerMass = 10f;
    /// <summary>
    /// Gets or sets a value indicating whether this instance is splitted.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is splitted; otherwise, <c>false</c>.
    /// </value>
    public bool IsSplitted { get; set; } = false;
    /// <summary>
    /// Gets or sets a value indicating whether this instance is merging allowed.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is merging allowed; otherwise, <c>false</c>.
    /// </value>
    public bool IsMergingAllowed { get; set; } = false;
    /// <summary>
    /// The player
    /// </summary>
    Player player;

    /// <summary>
    /// Occurs when [on score changed].
    /// </summary>
    public event Action<int> OnScoreChanged;


    /// <summary>
    /// Gets or sets the player score.
    /// </summary>
    /// <value>
    /// The player score.
    /// </value>
    public int PlayerScore
    {
        get { return playerScore; }
        set
        {
            if (playerScore != value)
            {
                playerScore = value;
                OnScoreChanged?.Invoke(playerScore);
            }
        }
    }

    /// <summary>
    /// Gets or sets the player mass.
    /// </summary>
    /// <value>
    /// The player mass.
    /// </value>
    public float PlayerMass
    {
        get { return playerMass; }
        set { playerMass = value; }
    }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        player = GetComponent<Player>();
        player.transform.localScale = new Vector3(player.PlayerMass * 0.1f, player.PlayerMass * 0.1f, player.PlayerMass * 0.1f);
    }

    /// <summary>
    /// Updates the scale.
    /// </summary>
    public void UpdateScale()
    {
        player = GetComponent<Player>();
        player.transform.localScale = Vector3.MoveTowards(player.transform.localScale, new Vector3(playerMass * 0.1f, playerMass * 0.1f, playerMass * 0.1f), 0.001f * playerMass);
    }

}
