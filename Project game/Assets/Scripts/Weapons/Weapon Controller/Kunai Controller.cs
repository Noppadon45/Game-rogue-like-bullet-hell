using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiController : WeaponsController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKunai = Instantiate(prefab);
        spawnedKunai.transform.position = transform.position;
        spawnedKunai.GetComponent<KunaiBehaviour>().DirectionChecker(pm.LastMovementVector);
    }
}