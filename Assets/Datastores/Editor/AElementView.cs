using UnityEngine.UIElements;

namespace Datastores
{
    /// <summary>
    /// The Visual Element that represents the element in the ListView of the DatastoreWindow.
    /// </summary>
    public abstract class AElementView : VisualElement
    {
        protected IDatastoreElement m_datastoreElement;
        protected int m_elementIndex;

        public AElementView()
        {
            CreateView();
        }

        public void SetElement(IDatastoreElement datastoreElement, int index)
        {
            m_datastoreElement = datastoreElement;
            m_elementIndex = index;
            OnSetElement();
        }

        /// <summary>
        /// Create/load the contents of the view here.
        /// </summary>
        protected abstract void CreateView();

        /// <summary>
        /// Bind the view to an IDatastoreElement.
        /// The view gets reused and rebound to different elements so this function should
        /// update the view to reflect the latest element.
        /// </summary>
        protected abstract void OnSetElement();
    }
}