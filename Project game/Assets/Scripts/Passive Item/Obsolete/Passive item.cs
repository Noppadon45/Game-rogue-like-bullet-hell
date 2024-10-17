using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passiveitem : MonoBehaviour
{
    protected PlayerStats Player;
    public PassiveitemScriptableScript PassiveData;
    // Start is called before the first frame update

    protected virtual void ApplyModifiler() //Apply the boots value stats
    {

    }
    void Start()
    {
        Player = FindObjectOfType<PlayerStats>();
        ApplyModifiler();
    }

   
}
