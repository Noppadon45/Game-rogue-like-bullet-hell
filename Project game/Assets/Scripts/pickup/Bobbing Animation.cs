using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frenquency;    //Speed object moving Up and Down
    public float magnitude;     //Range object moving Up and Down
    public Vector3 direction;   //Direction object moving
    Vector3 initPosition;
    Pickup pickup;

    void Start()
    {
        pickup = GetComponent<Pickup>();
        initPosition = transform.position;      //Save position starting point
    }

    void Update()
    {
        if (pickup && !pickup.IsCollect)
        {
            //Animation Bobbing effect
            transform.position = initPosition + direction * Mathf.Sin(Time.time * frenquency) * magnitude;
        }
        
        
    }
}
