using System;
using Datastores;
using UnityEngine;

public class SampleFilterA : AListViewFilter
{
    public override string FilterName => "Filter A";

    public override bool Evaluate(IDatastoreElement datastoreElement)
    {
        return true;
    }

    public override void OnGUI(Action CallOnFilterChanged)
    {
        GUILayout.Label("Filter A");
    }

    public override void ResetFilter()
    {
        
    }
}

[Serializable]
public class SampleFilterB : AListViewFilter
{
    public override string FilterName => "Filter B";
    
    public override bool Evaluate(IDatastoreElement datastoreElement)
    {
        return true;
    }

    public override void OnGUI(Action CallOnFilterChanged)
    {
        GUILayout.Label("Filter B");
    }
    
    public override void ResetFilter()
    {
        
    }
}
