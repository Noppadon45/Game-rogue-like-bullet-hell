using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed;
    Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontalvector;
    [HideInInspector]
    public float lastVerticalvector;
    [HideInInspector]
    public Vector2 moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        
    }
    void FixedUpdate()
    {
        move();
    }

    void InputManagement() 
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        if (moveDirection.x != 0)
        {
            lastHorizontalvector = moveDirection.x;
        }
        if (moveDirection.y != 0)
        {
            lastVerticalvector = moveDirection.y;
        }

    }
    void move()
    {
        rb.velocity = new Vector2(moveDirection.x * MovementSpeed , moveDirection.y * MovementSpeed);
    }

}
