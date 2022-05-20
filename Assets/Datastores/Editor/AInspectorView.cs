using UnityEngine.UIElements;

namespace Datastores
{
    public abstract class AInspectorView : VisualElement
    {
        protected DatastoreWindow m_datastoreWindow;
        protected Datastore m_datastore;
        public IDatastoreElement SelectedElement { get; private set; }

        public AInspectorView()
        {
            CreateView();
        }

        public void Init(DatastoreWindow datastoreWindow, Datastore datastore)
        {
            m_datastoreWindow = datastoreWindow;
            m_datastore = datastore;
        }

        public void SetElement(IDatastoreElement element)
        {
            ResetView();
            SelectedElement = element;
            OnSetElement(SelectedElement);
        }

        protected abstract void CreateView();
        protected abstract void OnSetElement(IDatastoreElement selectedElement);
        protected abstract void ResetView();
    }
}