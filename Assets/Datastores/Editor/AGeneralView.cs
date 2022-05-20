using UnityEngine.UIElements;

namespace Datastores
{
    public abstract class AGeneralView : VisualElement
    {
        protected DatastoreWindow m_datastoreWindow;
        protected Datastore m_datastore;

        public AGeneralView()
        {
            CreateView();
        }

        public void Init(DatastoreWindow datastoreWindow, Datastore datastore)
        {
            m_datastoreWindow = datastoreWindow;
            m_datastore = datastore;
            OnSetDatastore();
        }

        protected abstract void CreateView();
        protected abstract void OnSetDatastore();
        protected abstract void ResetView();
    }
}