using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{

    Animator am;
    PlayerMovement pm;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is moving in any direction
        if (pm.moveDirection.x != 0 || pm.moveDirection.y != 0)
        {
            am.SetBool("Ismove", true);
            //Call function SpriteDirectionChecker
            SpriteDirectionChecker();
        }else
        {
            am.SetBool("Ismove", false);
        }
    }
    // Check which direction the sprite should be facing
    void SpriteDirectionChecker() {
        if (pm.lastHorizontalvector < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    
    
    }
}
