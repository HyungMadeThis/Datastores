using System;
using System.Collections;
using System.Collections.Generic;
using Datastores;
using UnityEditor;
using UnityEngine;

public class ArmorDatastore : BaseDatastore<ArmorElement>
{
    private List<IDatastoreElement> m_armorElements = new List<IDatastoreElement>();

    public override Dictionary<string, (Type, Type)> ContextToTabTypeLookup { get; } =
        new Dictionary<string, (Type, Type)>()
        {
            ["Default"] = (typeof(ArmorGeneralView), typeof(DefaultInspectorView))
        };

    public override void Init()
    {
        LoadElements();
    }

    public override void OnDataReloadRequested()
    {
        LoadElements();
    }

    private void LoadElements()
    {
        m_armorElements.Clear();
        string[] assetGuids = AssetDatabase.FindAssets("t:ArmorData");
        foreach (string guid in assetGuids)
        {
            m_armorElements.Add(new ArmorElement(guid));
        }   
    }

    public override List<IDatastoreElement> GetElements(string searchFieldValue)
    {
        return SortByName(DefaultApplySearchFieldValue(m_armorElements, searchFieldValue));
    }

    public override IDatastoreElement GetElementById(string id)
    {
        return m_armorElements.Find(x => x.ElementId == id);
    }
}
