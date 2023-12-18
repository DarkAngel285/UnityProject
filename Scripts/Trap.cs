using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            collider.GetComponent<Player>().Hit(damage);
    }
}
