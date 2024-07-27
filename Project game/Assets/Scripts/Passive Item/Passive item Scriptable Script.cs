using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PassiveitemScriptableScript" , menuName = "ScriptableObjects/Passive item")]

public class PassiveitemScriptableScript : ScriptableObject {

    [SerializeField]
    float multiply;

    public float Multiply { get => multiply; set => multiply = value; }

    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab;     //Prefab the next level 
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    new string name;        //Name of Weapon or Passive when getting Upgrade
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string nameDescription;     //Description of Weapon or Passive when getting Upgrade
    public string NameDescription { get => nameDescription; private set => nameDescription = value; }

    [SerializeField]
    Sprite Icon;        //Not mean modify in game just only in Editor
    public Sprite icon { get => Icon; private set => Icon = value; }


}
