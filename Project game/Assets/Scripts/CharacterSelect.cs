using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEditor;

public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect instance;     //Create instance
    public CharacterData characterData;     //collect character that data when selected

    private void Awake()
    {   
        //Check instance
        if (instance == null)
        {
            //Dont Destroy when load new scene
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy new instance when already instance
            Debug.Log("Character" + this + "Deleted");
            Destroy(gameObject);
        }
    }

    //method to retrieve the current character data
    public static CharacterData GetData()
    {
        //If instance and character data exist, return it
        if (instance && instance.characterData)
            return instance.characterData;
        else
        {
            #if UNITY_EDITOR
            // // In the editor, search all asset paths for CharacterData assets
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            List<CharacterData> characterDatas = new List<CharacterData>();
            foreach (string assetPath in allAssetPaths)
            {
                if (assetPath.EndsWith(".asset"))
                {
                    //Attempt to load CharacterData asset
                    CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath);
                    if (characterData != null)
                    {
                        characterDatas.Add(characterData);
                    }
                }
            }
            // If any CharacterData assets were found, return one at random
            if (characterDatas.Count > 0)
            {
                return characterDatas[Random.Range(0, characterDatas.Count)];
            }
            #endif
            // If not in editor or no assets found, try to find CharacterData objects in memory
            CharacterData[] characters = Resources.FindObjectsOfTypeAll<CharacterData>();
            
        }
        // Return null if no character data was found
        return null;
        
    }
    //method Set the selected character
    public void SelectCharecter(CharacterData character)
    {
        characterData = character;
    }

    //method Destroy the singleton instance
    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
