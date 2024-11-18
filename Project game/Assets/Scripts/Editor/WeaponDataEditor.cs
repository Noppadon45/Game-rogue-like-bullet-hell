using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    WeaponData weaponData;
    string[] weaponSubType;
    int selectWeaponSubType;

    void OnEnable()
    {
        //Catch the weapon data value
        weaponData = (WeaponData)target;

        //Retrive all weapon subtype and catch it
        System.Type baseType = typeof(Weapon);
        List<System.Type> subTypes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToList();

        //Add a None option in front
        List<string> subTypeString = subTypes.Select(t => t.Name).ToList();
        subTypeString.Insert(0 , "None");
        weaponSubType = subTypeString.ToArray();

        //Ensure the weapon is correct weapon subtype
        selectWeaponSubType = Math.Max(0 , Array.IndexOf(weaponSubType, weaponData.behaviour));
    }

    public override void OnInspectorGUI()
    {
        //Draw a dropdown in the Inspector
        selectWeaponSubType = EditorGUILayout.Popup("Behaviour", Math.Max(0, selectWeaponSubType) , weaponSubType);

        if (selectWeaponSubType > 0)
        {
            //Update the behaviour field
            weaponData.behaviour = weaponSubType[selectWeaponSubType].ToString();
            
            EditorUtility.SetDirty(weaponData);     //Mark the object to save
            DrawDefaultInspector();         //Draw the default inspector elements
        }
        
    }
}
