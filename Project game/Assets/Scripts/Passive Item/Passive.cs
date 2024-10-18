using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override bool DoLevelUp()
    {
        base.DoLevelUp();

        if (!CanLevelUp())
        {
            Debug.Log(string.Format("Cant Level Up {0} to Level {1} , max level of {2} already max" , name , currentLevel , maxLevel));
            return false;
        }

        currentBoosts += passiveData.GetLevelData(currentLevel++).boosts;
        return true;
        
    }


}
