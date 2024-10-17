using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;

public class WeaponDataEditor : Editor
{
    WeaponData weaponData;
    string[] weaponSubType;
    int selectWeaponSubType;

    void OnEnable()
    {
        weaponData = (WeaponData)target;

        System.Type baseType = typeof(Weapon);
        List<System.Type> subTypes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToList() ;

        List<string> subTypeString = subTypes.Select(t => t.Name).ToList();
        subTypeString.Insert(0 , "None");
        weaponSubType = subTypeString.ToArray();

        selectWeaponSubType = Math.Max(0 , Array.IndexOf(weaponSubType, weaponData.behaviour));
    }

    public override void OnInspectorGUI()
    {
        selectWeaponSubType = EditorGUILayout.Popup("Behaviour", Math.Max(0, selectWeaponSubType) , weaponSubType);

        if (selectWeaponSubType > 0)
        {
            weaponData.behaviour = weaponSubType[selectWeaponSubType].ToString();
            EditorUtility.SetDirty(weaponData);
            DrawDefaultInspector();
        }
        
    }
}
