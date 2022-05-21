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

        private TextField textField;

        protected override void CreateView()
        {
            m_elementLabel = new Label();
            Add(m_elementLabel);
            
            Button button = new Button(() => { textField.value = JsonUtility.ToJson(m_datastoreWindow.m_state, true); });
            button.text = "Refresh state";
            Add(button);
            
            textField = new TextField();
            textField.style.flexGrow = 1;
            Add(textField);
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