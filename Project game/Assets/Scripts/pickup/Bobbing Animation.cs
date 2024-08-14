using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frenquency;    //Speed object moving Up and Down
    public float magnitude;     //Range object moving Up and Down
    public Vector3 direction;   //Direction object moving
    Vector3 initPosition;

    void Start()
    {
        initPosition = transform.position;      //Save position starting point
    }

    void Update()
    {
        //Animation Bobbing effect
        transform.position = initPosition + direction * Mathf.Sin(Time.time * frenquency) * magnitude;
    }
}
