using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakbleProp : MonoBehaviour
{
    public float health;

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health < 0)
        {
            Kill();
        }
        
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
