using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    Player player;
    public float splitDistance = 0.5f;
    public int counter = 0;

    private void Update()
    {
        player = GetComponent<Player>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SplitPlayer();
        }

        player.UpdateScale();
    }

    private void SplitPlayer()
    {
        if (player != null)
        {
            Debug.Log("PLAYER BEFORE: " + player.PlayerMass.ToString());

            Player playerSplitted = Instantiate(player, transform.position + new Vector3(splitDistance, 0, 0), Quaternion.identity);
            player.PlayerMass = player.PlayerMass / 2f;
            playerSplitted.PlayerMass = player.PlayerMass;
            playerSplitted.tag = "playerSplitted";

            Debug.Log("PLAYER NOW: " + player.PlayerMass.ToString());
            Debug.Log("SPLITTED: " + playerSplitted.PlayerMass.ToString());

            StartCoroutine(MergeBallsAfterDelay(5f));
        }
    }

    private IEnumerator MergeBallsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] splittedPlayersObjects = GameObject.FindGameObjectsWithTag("playerSplitted");

        foreach (GameObject splittedPlayer in splittedPlayersObjects)
        {
            player.PlayerMass += splittedPlayer.GetComponent<Player>().PlayerMass;
            Destroy(splittedPlayer);
            player.UpdateScale();
        }
    }

}