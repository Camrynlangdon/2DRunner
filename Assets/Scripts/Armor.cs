using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{

    [SerializeField]
    public float armorAmount;

    public Player player;
    private bool hasGivinArmor = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasGivinArmor) return;
        hasGivinArmor = true;
        Destroy(gameObject);
        player.changeArmor(armorAmount);
    }

}
