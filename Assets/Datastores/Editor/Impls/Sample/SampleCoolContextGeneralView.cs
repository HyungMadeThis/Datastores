using UnityEngine.UIElements;

namespace Datastores.Sample
{
    public class SampleCoolContextGeneralView : AGeneralView
    {
        protected override void CreateView()
        {
            Add(new Label("Sample COOL CONTEXT GeneralView"));
        }

        protected override void OnSetDatastore()
        {
        }

        protected override void ResetView()
        {
        }
    }
}