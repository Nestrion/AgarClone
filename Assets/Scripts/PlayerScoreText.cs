using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreText : MonoBehaviour
{

    // referenced set through editor 
    TextMeshProUGUI text;

    public Player player;

    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        text.text = "Score: " + (player.PlayerScore - 11).ToString();
    }

}
