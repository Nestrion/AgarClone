using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Food : MonoBehaviour
{

    public void Consume()
    {
        Destroy(gameObject);
    }
}
