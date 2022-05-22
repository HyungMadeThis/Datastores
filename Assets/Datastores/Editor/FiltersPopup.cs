using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FiltersPopup : PopupWindowContent
{
    private const float WINDOW_WIDTH = 80f;
    private List<AListViewFilter> m_listViewFilters;
    private Action m_onFilterChanged;
    public FiltersPopup(List<AListViewFilter> filters, Action onFilterChanged)
    {
        m_listViewFilters = filters;
        m_onFilterChanged = onFilterChanged;
    }
    
    public override void OnGUI(Rect rect)
    {
        GUILayout.BeginArea(rect);
        GUILayout.Label("filters yo");
        foreach (AListViewFilter filter in m_listViewFilters)
        {
            GUILayout.BeginVertical(new GUIStyle("box"), GUILayout.Height(filter.GUIHeight));
            filter.OnGUI(m_onFilterChanged);
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }

    public override Vector2 GetWindowSize()
    {
        Vector2 size = new Vector2(WINDOW_WIDTH, 0);
        foreach (AListViewFilter filter in m_listViewFilters)
        {
            size.y += filter.GUIHeight;
        }

        return size;
    }
}
