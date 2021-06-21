using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    int meleeDamage;



    public Player player;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.ChangeHealth(meleeDamage);
    }


}
