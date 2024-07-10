using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats Player;
    CircleCollider2D Magnet;
    public float pullforce;

    private void Start()
    {
        Player = FindObjectOfType<PlayerStats>();
        Magnet = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        Magnet.radius = Player.CurrentMagnet;
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        //check other game object Icollectable
        if (collision.gameObject.TryGetComponent(out Icollectable collectible))
        {
            //get Component Rigibody2D
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            //Add Item can be pull from position item to player positon
            Vector2 forceDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(forceDirection * pullforce);


            //If yes call Collect method
            collectible.Collect();
        }
    }
}
