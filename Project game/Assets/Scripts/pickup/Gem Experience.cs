using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemExperience : Pickup
{
    public int ExperienceGrant;
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
        player.IncreaseExperience(ExperienceGrant);
    }
    


}
