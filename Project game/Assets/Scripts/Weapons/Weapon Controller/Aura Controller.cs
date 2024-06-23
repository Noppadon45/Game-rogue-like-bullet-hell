using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraController : WeaponsController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject SpawnAura = Instantiate(prefab);
        SpawnAura.transform.position = transform.position;      //Assign position to the same Player
        SpawnAura.transform.parent = transform;                //Spawn Below Player
    }

}
