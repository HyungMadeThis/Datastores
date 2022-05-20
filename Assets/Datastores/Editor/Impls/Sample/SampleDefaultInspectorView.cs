using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Datastores.Sample
{
    /// <summary>
    /// The class that provides the inspector view of a selected element.
    /// This is a VisualElement!
    /// </summary>
    public class SampleDefaultInspectorView : AInspectorView
    {
        private Label m_elementLabel;

        public SampleDefaultInspectorView()
        {
            m_elementLabel = new Label();
            Add(m_elementLabel);
        }

        protected override void CreateView()
        {
        }

        protected override void OnSetElement(IDatastoreElement selectedElement)
        {
            m_elementLabel.text = "Sample Base Inspector for " + selectedElement.ElementName;
        }

        protected override void ResetView()
        {
        }
    }
}