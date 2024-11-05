using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int playerScore = 10;
    private float playerMass = 10f;
    public bool IsSplitted { get; set; } = false;
    public bool IsMergingAllowed { get; set; } = false;
    Player player;

    public int PlayerScore
    {
        get { return playerScore; }
        set { playerScore = value; }
    }

    public float PlayerMass
    {
        get { return playerMass; }
        set { playerMass = value; }
    }

    private void Start()
    {
        player = GetComponent<Player>();
        player.transform.localScale = new Vector3(player.PlayerMass * 0.1f, player.PlayerMass * 0.1f, player.PlayerMass * 0.1f);
    }

    public void UpdateScale()
    {
        player = GetComponent<Player>();
        player.transform.localScale = Vector3.MoveTowards(player.transform.localScale, new Vector3(playerMass * 0.1f, playerMass * 0.1f, playerMass * 0.1f), 0.001f * playerMass);
    }

}
