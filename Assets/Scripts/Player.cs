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
    /// Const scale shift
    /// </summary>
    private const float massConstScaleShift = 1f;
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
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        UpdateScale();
    }

    /// <summary>
    /// Updates the scale.
    /// </summary>
    public void UpdateScale()
    {
        GameCircle gameCircle = GetComponent<GameCircle>();
        if (gameCircle != null)
        {
            gameCircle.transform.localScale = new Vector3(gameCircle.GameCircleSizeScale(),
                                                          gameCircle.GameCircleSizeScale(),
                                                          gameCircle.GameCircleSizeScale());
        }
        else
        {
            Debug.Log("no game circle");
        }
    }
}
