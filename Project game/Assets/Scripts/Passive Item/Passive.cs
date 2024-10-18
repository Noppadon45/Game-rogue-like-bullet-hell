using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class take PassiveData and it use in a player stats when receive
public class Passive : Item
{
    public PassiveData passiveData;
    [SerializeField] CharacterData.Stats currentBoosts;

    [System.Serializable]
    public struct Modifier
    {
        public string name;
        public string description;
        public CharacterData.Stats boosts;
    }

    //create passive and set in initialise
    public virtual void Initialise(PassiveData Data)
    {
        base.Initialise(Data);
        this.passiveData = Data;
        currentBoosts = Data.baseStats.boosts;

    }

    public virtual CharacterData.Stats GetBoosts()
    {
        return currentBoosts;
    }

    //LevelUp the weapon by 1 and calulate the stats with weapon
    public override bool DoLevelUp()
    {
        base.DoLevelUp();

        //When cant levelup when item is max
        if (!CanLevelUp())
        {
            Debug.Log(string.Format("Cant Level Up {0} to Level {1} , max level of {2} already max" , name , currentLevel , maxLevel));
            return false;
        }
        
        //add stats of the next level to the weapon
        currentBoosts += passiveData.GetLevelData(currentLevel++).boosts;
        return true;
        
    }


}
