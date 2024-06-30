using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPassiveitem : Passiveitem
{
    protected override void ApplyModifiler()
    {
        Player.currentMoveSpeed *= 1 + PassiveData.Multiply / 100f;     //Ex Multiply 100 = 1 + 100/100 = 1+1 = 2
    }
}
