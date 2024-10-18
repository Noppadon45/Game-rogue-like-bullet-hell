using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("Passive Data") , menuName =("Passive Data"))]
public class PassiveData : ItemData
{
    public Passive.Modifier baseStats;
    public Passive.Modifier[] growth;

    public Passive.Modifier GetLevelData(int  level)
    {
        if (level - 2 < growth.Length)
        {
            return growth[level - 2];
        }
        Debug.Log(string.Format("Passive dont have level {0} for levle up" , level));
        return new Passive.Modifier();
    }
}
