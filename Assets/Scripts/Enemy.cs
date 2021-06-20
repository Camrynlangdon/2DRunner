using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    int spikedamage;

    public Player player;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.ChangeHealth(spikedamage);
    }
}
