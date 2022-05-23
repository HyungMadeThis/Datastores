using System;
using System.Collections;
using System.Collections.Generic;
using Datastores;
using UnityEngine;

[Serializable]
public abstract class AListViewFilter
{
    public abstract string FilterName { get; }
    public virtual float GUIHeight { get; } = 16f;
    public abstract bool Evaluate(IDatastoreElement datastoreElement);
    public abstract void OnGUI(Action CallOnFilterChanged);
    public abstract void ResetFilter();
}
