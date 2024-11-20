using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public Player player;
    public LeaderboardTable table;

    private void Start()
    {
        if (player != null)
        {
            table.LocalPlayerEntryName = player.playerName;
            player.OnScoreChanged += table.UpdateEntryForLocalPlayer;
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnScoreChanged -= table.UpdateEntryForLocalPlayer;
        }
    }
}
