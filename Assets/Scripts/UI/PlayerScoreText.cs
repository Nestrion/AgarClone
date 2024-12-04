using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 
/// </summary>
public class PlayerScoreText : MonoBehaviour
{

    // referenced set through editor 
    /// <summary>
    /// The text
    /// </summary>
    TextMeshProUGUI text;

    /// <summary>
    /// The player
    /// </summary>
    public Player player;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }


    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update()
    {
        text.text = "Score: " + player.PlayerScore.ToString();
    }

}
