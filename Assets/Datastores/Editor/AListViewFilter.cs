using System;
using System.Collections;
using System.Collections.Generic;
using Datastores;
using UnityEngine;

[Serializable]
public abstract class AListViewFilter
{
    public abstract float GUIHeight { get; }
    public abstract bool Evaluate(IDatastoreElement datastoreElement);
    public abstract void OnGUI(Action CallOnFilterChanged);
}
