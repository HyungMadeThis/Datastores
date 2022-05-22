using System;
using System.Collections.Generic;

namespace Datastores.Sample
{
    /// <summary>
    /// Simple impl of a Datastore. More comments in Datastore.cs
    /// </summary>
    public class SampleDatastore : BaseDatastore<SampleElement>
    {
        public override Type ElementViewType => typeof(SampleElementView);
        public override int ElementViewHeight => 30;

        public override Dictionary<string, (Type, Type)> ContextToTabTypeLookup { get; } = new Dictionary<string, (Type, Type)>()
        {
            ["Base"] =          (typeof(SampleDefaultGeneralView), typeof(SampleDefaultInspectorView)),
            ["Cool Context"] =  (typeof(SampleCoolContextGeneralView), typeof(SampleCoolContextInspectorView))
        };

        public override List<Type> ListViewFilterTypes { get; } = new List<Type>()
        {
            typeof(SampleFilterA),
            typeof(SampleFilterB)
        };

        private List<IDatastoreElement> m_elements = new List<IDatastoreElement>();

        public override void Init()
        {
            for (int i = 0; i < 100; i++)
            {
                m_elements.Add(new SampleElement() { ElementId = i.ToString() });
            }
        }

        /// <summary>
        /// This function gets called whenever Datastores needs the list of elements with the search and filters applied.
        /// So we need to apply the search and filters before returning it!!
        /// A simple impl of filtering is provided here.
        /// </summary>
        public override List<IDatastoreElement> GetElements(string searchFieldValue) // TODO: Pass in filters when implemented !!!!
        {
            return SortByName(DefaultApplySearchFieldValue(m_elements, searchFieldValue));
        }

        /// <summary>
        /// A simple get elements by ID.
        /// Only the datastore impl class (this class) has access to m_elements so this function needs to be implemented at this level every time.
        /// Any maybe in different circumstances we can do better than a linq .Find :o
        /// </summary>
        public override IDatastoreElement GetElementById(string id)
        {
            return m_elements.Find(x => x.ElementId == id);
        }
    }
}