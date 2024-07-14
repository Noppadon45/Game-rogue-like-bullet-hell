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
    void FixedUpdate()
    {
        move();
    }

    void InputManagement() 
    {
        if (GameManager.instance.IsGameOver)        //When GameOver Player Cant MoveDirection
        {
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        if (moveDirection.x != 0)
        {
            lastHorizontalvector = moveDirection.x;
            LastMovementVector = new Vector2 (lastHorizontalvector, 0f);    //LastmoveX
        }
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
    void move()
    {
        if (GameManager.instance.IsGameOver)    //When GameOver Player Cant MoveDirection
        {
            return;
        }

        rb.velocity = new Vector2(moveDirection.x * playerStats.CurrentMoveSpeed, moveDirection.y * playerStats.CurrentMoveSpeed);
    }

}
