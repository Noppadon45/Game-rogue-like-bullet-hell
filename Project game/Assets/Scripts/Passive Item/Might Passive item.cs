using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightPassiveitem : Passiveitem
{
    protected override void ApplyModifiler()
    {
        Player.CurrentMight *= 1 + PassiveData.Multiply / 100f;
    }
}
