using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect instance;
    public CharacterData characterData;

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
    public static CharacterData GetData()
    {
        if (instance && instance.characterData)
        {
            return instance.characterData;
        }
        else
        {
            CharacterData[] characters = Resources.FindObjectsOfTypeAll<CharacterData>();
            if (characters.Length > 0)
            {
                return characters[Random.Range(0, characters.Length)];
            }
        }
        return null;
        
    }
    public void SelectCharecter(CharacterData charecter)
    {
        characterData = charecter;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
