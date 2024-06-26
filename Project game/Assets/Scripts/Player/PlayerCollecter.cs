using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollecter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {

        //check other game object Icollectable
        if (collision.gameObject.TryGetComponent(out Icollectable collectible))
        {
            //If yes call Collect method
            collectible.Collect();
        }
    }
}
