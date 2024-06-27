using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If item enter to Player then Destroy
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
