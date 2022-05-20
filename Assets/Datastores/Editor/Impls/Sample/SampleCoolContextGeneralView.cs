using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Datastores.Sample
{
    public class SampleCoolContextGeneralView : AGeneralView
    {
        public SampleCoolContextGeneralView()
        {
            Add(new Label("Sample COOL CONTEXT GeneralView"));
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