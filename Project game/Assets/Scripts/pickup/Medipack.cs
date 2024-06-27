using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Medipack : Pickup , Icollectable
{
    public int HealRestore;
    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.Heal(HealRestore);
    }




}
