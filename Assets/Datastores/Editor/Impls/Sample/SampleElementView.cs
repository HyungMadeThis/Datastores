using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Datastores.Sample
{
    /// <summary>
    /// The class that provides the visual representation of an element when it's in the Datastore's listview.
    /// This is a VisualElement!
    /// </summary>
    public class SampleElementView : AElementView
    {
        private Label m_nameLabel;

        protected override void CreateView()
        {
            m_nameLabel = new Label();
            Add(m_nameLabel);
        }

        protected override void OnSetElement()
        {
            m_nameLabel.text = m_datastoreElement.ElementName;
        }
    }
}