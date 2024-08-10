using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Medipack : Pickup 
{
    public int HealRestore;
    public override void Collect()
    {
        if (IsCollect)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.Heal(HealRestore);
    }




}
