using System;
using System.Collections;
using System.Collections.Generic;
using Datastores;
using UnityEngine;

public class SampleFilterA : AListViewFilter
{
    public bool samplefilterabool;
    public override float GUIHeight => 9f;

    public override bool Evaluate(IDatastoreElement datastoreElement)
    {
        return true;
    }

    public override void OnGUI(Action CallOnFilterChanged)
    {
        GUILayout.Label("Filter A");
    }
}

[Serializable]
public class SampleFilterB : AListViewFilter
{
    public bool samplefilterbbool;
    public override float GUIHeight => 40f;
    
    public override bool Evaluate(IDatastoreElement datastoreElement)
    {
        return true;
    }

    public override void OnGUI(Action CallOnFilterChanged)
    {
        GUILayout.Label("Filter B");
    }
}
