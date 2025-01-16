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
        Player[] playerCircles = FindObjectsOfType<Player>();
        int totalScore = 0;
        foreach (var circle in playerCircles)
        {
            if (circle.isActiveAndEnabled)
                totalScore += circle.PlayerScore;
        }
        text.text = "Score: " + totalScore;
    }

}
