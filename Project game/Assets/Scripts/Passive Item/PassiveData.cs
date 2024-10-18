using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class for store stats of PassiveData in Game


[CreateAssetMenu(fileName = ("Passive Data") , menuName =("Passive Data"))]
public class PassiveData : ItemData
{
    public Passive.Modifier baseStats;
    public Passive.Modifier[] growth;

    public Passive.Modifier GetLevelData(int  level)
    {
        //Pick the Stats from the next Level
        if (level - 2 < growth.Length)
        {
            return growth[level - 2];
        }
        //Return an empty value and warning
        Debug.Log(string.Format("Passive dont have level {0} for levle up" , level));
        return new Passive.Modifier();
    }
}
