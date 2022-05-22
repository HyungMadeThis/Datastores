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
                DatastoreStates.Add(datastoreState);
            }
            return datastoreState;
        }
    }

    [Serializable]
    public class DatastoreState
    {
        public string DatastoreTypeName = string.Empty;
        public string SelectedContext = string.Empty;
        public string SelectedElementId = string.Empty;
        public string SearchFieldValue = string.Empty;
        [SerializeReference]
        public List<AListViewFilter> Filters = new List<AListViewFilter>();
        public float ListViewPosition;
        public float SplitViewPosition = 210f;
        public DatastoreWindow.SelectedTab SelectedTab = DatastoreWindow.SelectedTab.GENERAL;
        public float ScrollViewPosition;
    }
}