using System;
using System.Collections.Generic;

namespace Datastores
{
    public class NullDatastore : BaseDatastore<NullDatastoreElement>
    {
        List<IDatastoreElement> m_elements = new List<IDatastoreElement>();
        public override void Init()
        {
            for (int i = 0; i < 1; i++)
            {
                m_elements.Add(new NullDatastoreElement() { ElementId = i.ToString() });
            }
        }
        
        public override List<Type> ListViewFilterTypes { get; } = new List<Type>()
        {
            typeof(SampleFilterA),
        };

        public override List<IDatastoreElement> GetElements(string searchFieldValue)
        {
            return new List<IDatastoreElement>(m_elements);
        }

        public override IDatastoreElement GetElementById(string id)
        {
            return m_elements.Find(x => x.ElementId == id);
        }
    }

    public class NullDatastoreElement : IDatastoreElement
    {
        public string ElementName { get { return "Element " + ElementId; } }
        public string ElementId { get; set; }
    }
}