using System;
using System.Collections.Generic;
using System.Linq;
using Datastores.Sample;

namespace Datastores
{
    /// <summary>
    /// Inherit this GENERIC class!! Don't inherit from the non-generic Datastore class!!!@!@@!
    /// </summary>
    public abstract class BaseDatastore<T> : Datastore where T : IDatastoreElement { }

    /// <summary>
    /// The base class of a datastore.
    /// </summary>
    public abstract class Datastore
    {
        /// <summary>
        /// The class type to use to draw elements in the list view.
        /// </summary>
        public virtual Type ElementViewType { get; } = typeof(DefaultElementView);

        /// <summary>
        /// We need to specify element height here because UIToolkit.ListView does not support dynamic sizes in Unity2020.3. It does in Unity2021 though!!
        /// </summary>
        public virtual int ElementViewHeight { get; } = 16;

        /// <summary>
        /// Hard code the connections between context and the type of views to show for the General and Inspector tabs.
        /// We COULD search through all assemblies and auto find the classes but that would add seconds to the compile time in big
        /// projects and that is NO GOOD.
        /// So instead, every new Datastore type should define this dictionary manually by overriding this field.
        /// 
        /// Key: Context name
        /// Value: Tuple of (GeneralView class type, InspectorView class type)
        /// </summary>
        public virtual Dictionary<string, (Type, Type)> ContextToTabTypeLookup { get; } = new Dictionary<string, (Type, Type)>()
        {
            ["Default"] = (typeof(DefaultGeneralView), typeof(SampleDefaultInspectorView)),
            ["Default2"] = (typeof(DefaultGeneralView), typeof(DefaultInspectorView))
        };

        /// <summary>
        /// The  filters that are applicable to this datastore by type. The reason for hardcoding these is the same as the context dictionary above.
        /// </summary>
        public virtual List<Type> ListViewFilterTypes { get; } = new List<Type>();

        public abstract void Init();

        /// <summary>
        /// Every datastore needs to be able to provide a list of its elements WITH search and filters applied.
        /// Because only the inherited class has access to all the elements, this function must exist there rather than here.
        /// TODO: Possibly make this async
        /// TODO: Support filters
        /// </summary>
        public abstract List<IDatastoreElement> GetElements(string searchFieldValue);

        /// <summary>
        /// Every datastore needs to be able to grab elements by Id. 
        /// Because only the inherited class has access to all the elements, this function must exist there rather than here.
        /// </summary>
        public abstract IDatastoreElement GetElementById(string id);

        /// <summary>
        /// Simple sorters.
        /// </summary>
        protected List<IDatastoreElement> SortByName(List<IDatastoreElement> elements)
        {
            return elements.OrderBy(x => x.ElementName).ToList();
        }
        protected List<IDatastoreElement> SortById(List<IDatastoreElement> elements)
        {
            return elements.OrderBy(x => x.ElementId).ToList();
        }

        /// <summary>
        /// The default behavior of the search field. Individual implementations of Datastore can override this!
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="searchFieldValue"></param>
        /// <returns></returns>
        protected List<IDatastoreElement> DefaultApplySearchFieldValue(List<IDatastoreElement> elements, string searchFieldValue)
        {
            if (string.IsNullOrEmpty(searchFieldValue))
            {
                return elements;
            }

            string[] searchArgs = searchFieldValue.ToLower().Split(' ');

            List<IDatastoreElement> matches = new List<IDatastoreElement>();
            for(int i = 0; i < elements.Count; i++)
            {
                string elementName = elements[i].ElementName.ToLower();
                bool noMatch = false;
                for (int j = 0; j < searchArgs.Length; j++)
                {
                    int index = elementName.IndexOf(searchArgs[j]);
                    if(index == -1)
                    {
                        noMatch = true;
                        continue;
                    }
                    elementName = elementName.Remove(index, searchArgs[j].Length);
                }
                if(!noMatch)
                {
                    matches.Add(elements[i]);
                }
            }

            return matches;
        }
    }
}