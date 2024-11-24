using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEditor;

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
        }
        else
        {
            Debug.Log("Character" + this + "Deleted");
            Destroy(gameObject);
        }
    }
    public static CharacterData GetData()
    {
        if (instance && instance.characterData)
            return instance.characterData;
        else
        {
            #if UNITY_EDITOR
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            List<CharacterData> characterDatas = new List<CharacterData>();
            foreach (string assetPath in allAssetPaths)
            {
                if (assetPath.EndsWith(".asset"))
                {
                    CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath);
                    if (characterData != null)
                    {
                        characterDatas.Add(characterData);
                    }
                }
            }

            if (characterDatas.Count > 0)
            {
                return characterDatas[Random.Range(0, characterDatas.Count)];
            }
            #endif
            CharacterData[] characters = Resources.FindObjectsOfTypeAll<CharacterData>();
            
        }
        return null;
        
    }
    public void SelectCharecter(CharacterData character)
    {
        characterData = character;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
