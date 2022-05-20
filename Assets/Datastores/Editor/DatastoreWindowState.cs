using System;
using System.Collections.Generic;
using UnityEngine;

namespace Datastores
{
    [Serializable]
    public class DatastoreWindowState
    {
        public string SelectedDatastoreType;
        public List<DatastoreState> DatastoreStates = new List<DatastoreState>();

        public DatastoreState GetDatastoreState(Type datastoreType)
        {
            DatastoreState datastoreState = DatastoreStates.Find(x => x.DatastoreTypeName == datastoreType.Name);
            if(datastoreState == null)
            {
                datastoreState = new DatastoreState() { DatastoreTypeName = datastoreType.Name };
            }
            return datastoreState;
        }

        public void SetDatastoreState(DatastoreState datastoreState)
        {
            int index = DatastoreStates.FindIndex(x => x.DatastoreTypeName == datastoreState.DatastoreTypeName);
            if(index == -1)
            {
                DatastoreStates.Add(datastoreState);
            }
            else
            {
                DatastoreStates[index] = datastoreState;
            }
        }
    }

    [Serializable]
    public class DatastoreState
    {
        public string DatastoreTypeName = string.Empty;
        public string SelectedContext = string.Empty;
        public string SelectedElementId = string.Empty;
        public string SearchFieldValue = string.Empty;
        public string Filters = string.Empty;
        public float ListViewPosition;
        public float SplitViewPosition = 210f;
        public DatastoreWindow.SelectedTab SelectedTab = DatastoreWindow.SelectedTab.GENERAL;
        public float ScrollViewPosition;
    }
}