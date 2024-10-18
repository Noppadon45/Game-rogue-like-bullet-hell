using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : Item
{
    public Passiveitem passiveItemData;
    [SerializeField] CharacterData.Stats currentBoosts;

    [System.Serializable]
    public struct Modifier
    {
        public string name;
        public string description;
        public CharacterData.Stats boosts;
    }

    
}
