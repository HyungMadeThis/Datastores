using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Datastores
{
    public class DatastoreWindow : EditorWindow, IHasCustomMenu
    {
        [Serializable]
        public enum SelectedTab
        {
            GENERAL = 0,
            INSPECTOR = 1
        }

        // statics
        private static readonly string DatastoresWindowUXML = "Datastores/DatastoresWindowUXML";
        private static readonly string DatastoreToolbarMenu_TAG = "datastore-toolbar-menu";
        private static readonly string ContextToolbarMenu_TAG = "context-toolbar-menu";
        private static readonly string SearchField_TAG = "filter-search-field";
        private static readonly string FiltersButton_TAG = "filters-button";
        private static readonly string ElementsList_TAG = "elements-list";
        private static readonly string ListAreaScroller_CLASSNAME = "unity-scroller--vertical";
        private static readonly string TotalElementsLabel_TAG = "total-elements-label";
        private static readonly string SplitView_TAG = "split-view";
        private static readonly string InspectorTab_TAG = "inspector-tab";
        private static readonly string GeneralTab_TAG = "general-tab";
        private static readonly string RightPanel_ScrollView_TAG = "scroll-view";
        private static Color SelectedTab_COLOR = new Color(0.2196f, 0.2196f, 0.2196f); // cool hardcoded colors yey
        private static Color UnselectedTab_COLOR = new Color(0.1647f, 0.1647f, 0.1647f);

        // visual elements
        private ToolbarMenu m_datastoreToolbarMenu;
        private ToolbarMenu m_contextToolbarMenu;
        private ToolbarSearchField m_searchField;
        private Button m_filtersButton; // creates a PopupWindow
        private ListView m_elementListView;
        private Scroller m_listViewScroller;
        private Label m_totalElementsLabel;
        private TwoPaneSplitView m_splitView;
        private VisualElement m_inspectorTab;
        private VisualElement m_generalTab;
        private ScrollView m_rightPanelScrollView;

        // variables
        private List<Type> m_datastoreTypes = new List<Type>();
        private Dictionary<Type, Datastore> m_datastoreInstances = new Dictionary<Type, Datastore>();
        private Datastore m_selectedDatastore;
        [SerializeField]
        public DatastoreWindowState m_state = new DatastoreWindowState(); //TODO: public for testing  
        private DatastoreState m_datastoreState;
        private List<IDatastoreElement> m_cachedElementsList = new List<IDatastoreElement>();
        private AGeneralView m_generalView;
        private AInspectorView m_inspectorView;

        #region Init
        [MenuItem("Hyung/Datastores")]
        public static void OpenWindow()
        {
            DatastoreWindow window = GetWindow<DatastoreWindow>("Datastores");
            window.minSize = new Vector2(371,300);
            window.Show();
        }

        private void OnEnable()
        {
            CreateLayout();
            SetupDatastoresDropdown();
            SetupCallbacks();

            Type selectedDatastoreType = m_datastoreTypes.Find(x => x.Name == m_state.SelectedDatastoreType);
            LoadDatastore(selectedDatastoreType);
        }

        private void CreateLayout()
        {
            //==================================Load Initial Data======================================//
            var uxmlAsset = Resources.Load<VisualTreeAsset>(DatastoresWindowUXML);
            uxmlAsset.CloneTree(rootVisualElement);

            m_datastoreToolbarMenu = rootVisualElement.Q<ToolbarMenu>(DatastoreToolbarMenu_TAG);
            m_contextToolbarMenu = rootVisualElement.Q<ToolbarMenu>(ContextToolbarMenu_TAG);
            m_searchField = rootVisualElement.Q<ToolbarSearchField>(SearchField_TAG);
            m_filtersButton = rootVisualElement.Q<Button>(FiltersButton_TAG);
            m_elementListView = rootVisualElement.Q<ListView>(ElementsList_TAG);
            m_totalElementsLabel = rootVisualElement.Q<Label>(TotalElementsLabel_TAG);
            m_splitView = rootVisualElement.Q<TwoPaneSplitView>(SplitView_TAG);
            m_inspectorTab = rootVisualElement.Q<VisualElement>(InspectorTab_TAG);
            m_generalTab = rootVisualElement.Q<VisualElement>(GeneralTab_TAG);
            m_rightPanelScrollView = rootVisualElement.Q<ScrollView>(RightPanel_ScrollView_TAG);
            //=========================================================================================//
            
            m_listViewScroller = m_elementListView.Q<Scroller>(className: ListAreaScroller_CLASSNAME); // direct access to the scrollview
            m_elementListView.onSelectionChange += OnElementSelected;
            m_elementListView.showBoundCollectionSize = false;
        }

        private void SetupDatastoresDropdown()
        {
            // First, grab all classes that inherit from Datastore.
            Type baseDatastoreType = typeof(Datastore);
            m_datastoreTypes = Assembly.GetAssembly(typeof(Datastore)).GetTypes().Where(
                x => baseDatastoreType.IsAssignableFrom(x)
                && !x.IsAbstract
                && x.IsClass).OrderBy(y => y.Name).ToList(); // TODO: Are we guaranteed all inherited classes are in this Assembly? Probably not...

            // Second, populate the dropdown and wire in the callbacks
            foreach (Type datastoreType in m_datastoreTypes)
            {
                m_datastoreToolbarMenu.menu.AppendAction(datastoreType.Name, (something) =>
                {
                    LoadDatastore(datastoreType);
                });
            }
        }

        private void SetupCallbacks()
        {
            m_generalTab.RegisterCallback<MouseDownEvent>((mouseDownEvent) => { SelectTab(SelectedTab.GENERAL); });
            m_inspectorTab.RegisterCallback<MouseDownEvent>((mouseDownEvent) => { SelectTab(SelectedTab.INSPECTOR); });
            m_searchField.RegisterValueChangedCallback((changeEvent) => { PopulateListView(changeEvent.newValue); });
            m_splitView.Q<VisualElement>("unity-content-container")[m_splitView.fixedPaneIndex].RegisterCallback<GeometryChangedEvent>(
                (evt) => { m_datastoreState.SplitViewPosition = evt.newRect.width; });
            m_elementListView.RegisterCallback((ChangeEvent<float> evt) => { m_datastoreState.ListViewPosition = evt.newValue; });
            m_searchField.RegisterCallback((ChangeEvent<string> evt) => { m_datastoreState.SearchFieldValue = evt.newValue; });
        }
        #endregion

        private void LoadDatastore(Type datastoreType)
        {
            m_cachedElementsList.Clear();

            // if nothing is passed in, default to null datastore.
            if (datastoreType == null)
            {
                datastoreType = typeof(NullDatastore);
            }

            // Update dropdown selected datastore name
            m_datastoreToolbarMenu.text = datastoreType.Name;

            // Load datastore instance
            m_datastoreInstances.TryGetValue(datastoreType, out Datastore datastore);
            if(datastore == null)
            {
                datastore = Activator.CreateInstance(datastoreType) as Datastore;
                datastore.Init();
                m_datastoreInstances.Add(datastoreType, datastore);
            }
            m_selectedDatastore = datastore;

            // Setup contexts
            SetupContextsDropdown(m_selectedDatastore);

            // Update the bindings to the listview and empty it.
            m_elementListView.itemsSource = new List<IDatastoreElement>();
            m_elementListView.itemHeight = m_selectedDatastore.ElementViewHeight; //TODO: FIX THIS TO BE DYNAMIC HEIGHT!
            m_elementListView.makeItem = () => Activator.CreateInstance(m_selectedDatastore.ElementViewType) as VisualElement;
            m_elementListView.bindItem = (visualElement, index) => { ((AElementView)visualElement).SetElement((IDatastoreElement)m_elementListView.itemsSource[index], index); };

            // Load state
            m_state.SelectedDatastoreType = datastoreType.Name;
            m_datastoreState = m_state.GetDatastoreState(datastoreType);
            if (m_splitView.fixedPaneInitialDimension.Equals(m_datastoreState.SplitViewPosition))
            {
                // If we set the initial dimension to the same value, it doesnt actually refresh the splitview.
                // so we gotta set it to something different, and then re-set it to get it to guarantee refresh.
                // It's really stupid the splitview doesnt have a built in "refresh" function.
                m_splitView.fixedPaneInitialDimension = m_datastoreState.SplitViewPosition + 1;
            }
            m_splitView.fixedPaneInitialDimension = m_datastoreState.SplitViewPosition;
            m_searchField.value = m_datastoreState.SearchFieldValue;
            m_rightPanelScrollView.verticalScroller.value = m_datastoreState.ScrollViewPosition;

            PopulateListView(m_searchField.value);
            SelectContext(m_datastoreState.SelectedContext);
            EditorCoroutineUtility.StartCoroutine(LoadSelectedElement(m_datastoreState.SelectedElementId), this);
        }

        private void SetupContextsDropdown(Datastore datastore)
        {            
            // Clear the previous context dropdown. Is there a better way to do this?
            int menuItemsToClear = m_contextToolbarMenu.menu.MenuItems().Count;
            for(int i = 0; i < menuItemsToClear; i++)
            {
                m_contextToolbarMenu.menu.RemoveItemAt(0);
            }

            // Populate the contexts
            List<string> contexts = datastore.ContextToTabTypeLookup.Keys.ToList();

            foreach (string context in contexts)
            {
                m_contextToolbarMenu.menu.AppendAction(context, (something) =>
                {
                    SelectContext(context);
                });
            }
        }

        private void SelectTab(SelectedTab selectedTab)
        {
            m_generalTab.style.backgroundColor = selectedTab == SelectedTab.GENERAL ? SelectedTab_COLOR : UnselectedTab_COLOR;
            m_inspectorTab.style.backgroundColor = selectedTab == SelectedTab.INSPECTOR ? SelectedTab_COLOR : UnselectedTab_COLOR;

            m_generalView.style.display = selectedTab == SelectedTab.GENERAL ? DisplayStyle.Flex : DisplayStyle.None;
            m_inspectorView.style.display = selectedTab == SelectedTab.INSPECTOR ? DisplayStyle.Flex : DisplayStyle.None;

            if (m_datastoreState != null)
            {
                m_datastoreState.SelectedTab = selectedTab;
            }
        }

        private void SelectContext(string context)
        {
            // Sanitize the context.
            if (!m_selectedDatastore.ContextToTabTypeLookup.TryGetValue(context, out (Type, Type) TabTypes))
            {
                // If the context wasnt found but at least one context does exist, set the context to the first index.
                if (m_selectedDatastore.ContextToTabTypeLookup.Count > 0)
                {
                    context = m_selectedDatastore.ContextToTabTypeLookup.Keys.First();
                    TabTypes = m_selectedDatastore.ContextToTabTypeLookup[context];

                }
                else // If no contexts exist, just default.
                {
                    context = "";
                    TabTypes = (typeof(DefaultGeneralView), typeof(DefaultInspectorView)); // Default tab views
                }
            }

            // Save context to state
            m_contextToolbarMenu.text = context;
            if (m_datastoreState != null)
            {
                m_datastoreState.SelectedContext = context;
            }

            // Create general and inspector views
            if (m_generalView != null)
            {
                m_rightPanelScrollView.Remove(m_generalView);
            }

            m_generalView = Activator.CreateInstance(TabTypes.Item1) as AGeneralView;
            m_generalView.Init(this, m_selectedDatastore);
            m_rightPanelScrollView.Add(m_generalView);
            m_generalView.style.display = DisplayStyle.None; // Hide it by default.

            IDatastoreElement selectedElement = null; // If the previous inspector had a selected element, retain it and load it into the new inspector
            if (m_inspectorView != null)
            {
                selectedElement = m_inspectorView.SelectedElement;
                m_rightPanelScrollView.Remove(m_inspectorView);
            }
            m_inspectorView = Activator.CreateInstance(TabTypes.Item2) as AInspectorView;
            m_inspectorView.Init(this, m_selectedDatastore);
            m_rightPanelScrollView.Add(m_inspectorView);
            m_inspectorView.style.display = DisplayStyle.None;
            if(selectedElement != null)
            {
                m_inspectorView.SetElement(selectedElement);
            }

            SelectTab(m_datastoreState.SelectedTab);
        }

        /// <summary>
        /// To be called when new datastore is loaded or search/filters have changed.
        /// </summary>
        private void PopulateListView(string searchString)// TODO: pass in filters.
        {
            if(m_selectedDatastore == null)
            {
                return;
            }

            m_cachedElementsList = new List<IDatastoreElement>(m_selectedDatastore.GetElements(searchString));
            m_elementListView.itemsSource = m_cachedElementsList;
            //m_elementListView.RefreshItems();
            m_totalElementsLabel.text = $"{m_cachedElementsList.Count} Total Element" + (m_cachedElementsList.Count == 1 ? "" : "s");
        }

        /// <summary>
        /// This function needs to be "async" because we can't set the listview scroller position until its figured out the meaning of life.
        /// If we set the scroller position too early it looks like the listview just overrides it.
        /// If we don't want to use EditorCoroutines, we can turn this into an actual async function and call Task.Delay(1) but that option
        /// leads a visual artifact where the scroll is at position 0 for a few frames before it jumps to the correct position. Ugly.
        /// </summary>
        private IEnumerator LoadSelectedElement(string elementId)
        {
            if (!string.IsNullOrEmpty(elementId) && m_cachedElementsList.Count > 0)
            {
                int index = m_cachedElementsList.FindIndex(x => x.ElementId == elementId);
                if (index != -1)
                {
                    SelectedTab selectedTab = m_datastoreState != null ? m_datastoreState.SelectedTab : SelectedTab.GENERAL;
                    m_elementListView.SetSelection(index);
                    SelectTab(selectedTab); // m_elementListView.SetSelection() auto selects the Inspector tab but when loading state we want to retain the saved tab instead.

                    m_listViewScroller.value = m_datastoreState.ListViewPosition;
                    yield return new WaitForEndOfFrame();
                    //await System.Threading.Tasks.Task.Delay(1);
                    m_listViewScroller.value = m_datastoreState.ListViewPosition;
                }
                else
                {
                    m_inspectorView.SetElement(m_selectedDatastore.GetElementById(elementId));
                }
            }
        }

        private void OnElementSelected(IEnumerable<object> elements)
        {
            IDatastoreElement datastoreElement = elements.Count() == 0 ? null : elements.First() as IDatastoreElement;
            m_inspectorView.SetElement(datastoreElement);
            m_datastoreState.SelectedElementId = datastoreElement?.ElementId;
            SelectTab(SelectedTab.INSPECTOR);
        }

        /// <summary>
        /// For the three little dots at the top right of the editor window!
        /// </summary>
        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Log State"), false, () =>
            {
                Debug.Log($"Datastores State:\n {JsonUtility.ToJson(m_state, true)}");
            });
            menu.AddItem(new GUIContent("Clear Datastores State"), false, () =>
            {
                m_state = new DatastoreWindowState();
                m_datastoreState = null;
                LoadDatastore(null);
                Debug.Log("Cleared state.");
            });
        }
    }
}