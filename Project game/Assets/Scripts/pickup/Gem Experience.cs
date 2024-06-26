using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemExperience : MonoBehaviour , Icollectable
{
    public int ExperienceGrant;
    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(ExperienceGrant);
        Destroy(gameObject);
    }

    
}
