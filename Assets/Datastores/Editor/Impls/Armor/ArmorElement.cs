using System.Collections;
using System.Collections.Generic;
using Datastores;
using UnityEditor;
using UnityEngine;

public class ArmorElement : IDatastoreElement
{
    private ArmorData m_armorData;
    public string ElementName { get { return m_armorData.name; } }
    public string ElementId { get { return m_armorData.Id; } }

    public ArmorElement(string armorDataGuid)
    {
        m_armorData = AssetDatabase.LoadAssetAtPath<ArmorData>(AssetDatabase.GUIDToAssetPath(armorDataGuid));
    }
    
}