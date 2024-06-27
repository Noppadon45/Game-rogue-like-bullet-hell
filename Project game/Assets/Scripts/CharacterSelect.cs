using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect instance;
    public CharacterSciptableObject characterData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Debug.Log("Character" + this + "Deleted");
            Destroy(gameObject);
        }
    }
    public static CharacterSciptableObject GetData()
    {
        return instance.characterData;
    }
    public void SelectCharecter(CharacterSciptableObject charecter)
    {
        characterData = charecter;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
