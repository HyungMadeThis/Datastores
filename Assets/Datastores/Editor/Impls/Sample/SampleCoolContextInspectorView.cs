using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Datastores.Sample
{
    public class SampleCoolContextInspectorView : AInspectorView
    {
        private Label m_elementLabel;

        public SampleCoolContextInspectorView()
        {
            m_elementLabel = new Label();
            Add(m_elementLabel);
        }

        protected override void CreateView()
        {
        }

        protected override void OnSetElement(IDatastoreElement selectedElement)
        {
            m_elementLabel.text = $"Sample COOL CONTEXT Inspector for {selectedElement.ElementName}!!! WOWOWOWOWW!";
        }

        protected override void ResetView()
        {
        }
    }
}