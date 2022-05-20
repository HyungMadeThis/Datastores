using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Datastores.Sample
{
    /// <summary>
    /// The class that provides a general view of a datastore.
    /// This is a VisualElement!
    /// </summary>
    public class SampleDefaultGeneralView : AGeneralView
    {
        public SampleDefaultGeneralView()
        {
            Add(new Label("Sample Base GeneralView"));
        }

        protected override void CreateView()
        {
        }

        protected override void OnSetDatastore()
        {
        }

        protected override void ResetView()
        {
        }
    }
}