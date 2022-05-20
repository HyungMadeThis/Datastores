using UnityEngine.UIElements;

namespace Datastores
{
    public class DefaultElementView : AElementView
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

    public class DefaultGeneralView : AGeneralView
    {
        public DefaultGeneralView()
        {
            Add(new Label("DefaultGeneralView"));
        }

        protected override void CreateView() { }
        protected override void OnSetDatastore() { }
        protected override void ResetView() { }
    }

    public class DefaultInspectorView : AInspectorView
    {
        public DefaultInspectorView()
        {
            Add(new Label("DefaultInspectorView"));
        }

        protected override void CreateView() { }
        protected override void OnSetElement(IDatastoreElement selectedElement) { }
        protected override void ResetView() { }
    }
}