using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PassiveitemScriptableScript" , menuName = "ScriptableObjects/Passive item")]

public class PassiveitemScriptableScript : ScriptableObject {

    [SerializeField]
    float multiply;

    public float Multiply { get => multiply; set => multiply = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
