using UnityEngine.UIElements;

namespace Datastores.Sample
{
    /// <summary>
    /// The class that provides a general view of a datastore.
    /// This is a VisualElement!
    /// </summary>
    public class SampleDefaultGeneralView : AGeneralView
    {
        protected override void CreateView()
        {
            Add(new Label("Sample Base GeneralView"));
        }

        protected override void OnSetDatastore()
        {
        }

        protected override void ResetView()
        {
        }
    }
}