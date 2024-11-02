using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int playerScore = 10;

    private int PlayerScore
    {
        get { return playerScore; }
        set { playerScore = value; }
    }

}
