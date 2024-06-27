using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemExperience : Pickup , Icollectable
{
    public int ExperienceGrant;
    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(ExperienceGrant);
    }
    


}
