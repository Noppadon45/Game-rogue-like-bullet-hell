using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiBehaviour : ProjectileWeaponBehaviour
{
    KunaiController kc;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        kc = FindObjectOfType<KunaiController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * kc.Speed * Time.deltaTime;    //Set movement of kunai
    }
}
