using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed;
    [HideInInspector]
    public float lastHorizontalvector;
    [HideInInspector]
    public float lastVerticalvector;
    [HideInInspector]
    public Vector2 moveDirection;
    public Vector2 LastMovementVector;

    Rigidbody2D rb;
    PlayerStats playerStats;


    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        LastMovementVector = new Vector2(1, 0f);        //Default Startgame movement
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        
    }
    // Called at a fixed interval for physics-based movement
    void FixedUpdate()
    {
        move();
    }

    // Handle player input and store movement directions
    void InputManagement() 
    {
        if (GameManager.instance.IsGameOver)        //When GameOver Player Cant MoveDirection
        {
            return;
        }

        // Get raw input for X and Y 
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        // Store last horizontal input if present
        if (moveDirection.x != 0)
        {
            lastHorizontalvector = moveDirection.x;
            LastMovementVector = new Vector2 (lastHorizontalvector, 0f);    //LastmoveX
        }
        // Store last vertical input if present
        if (moveDirection.y != 0)
        {
            lastVerticalvector = moveDirection.y;
            LastMovementVector = new Vector2(0f, lastVerticalvector);   //LastmoveY
            {

            };
        }

        if (moveDirection.x != 0 && moveDirection.y != 0)
        {
            LastMovementVector = new Vector2(lastHorizontalvector, lastVerticalvector);     //While moving
        }

    }

    // Apply velocity to the Rigidbody2D to move the player
    void move()
    {
        if (GameManager.instance.IsGameOver)    //When GameOver Player Cant MoveDirection
        {
            return;
        }

        rb.velocity = new Vector2(moveDirection.x * playerStats.CurrentMoveSpeed, moveDirection.y * playerStats.CurrentMoveSpeed);
    }

}
