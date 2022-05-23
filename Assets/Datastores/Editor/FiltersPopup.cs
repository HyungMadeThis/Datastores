using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FiltersPopup : PopupWindowContent
{
    private const float WINDOW_WIDTH = 160f;
    private const float RESET_BUTTON_PADDING = 4f;
    private const float RESET_BUTTON_HEIGHT = 24f;
    private const float FILTER_NAME_HEIGHT = 24f;
    private List<AListViewFilter> m_listViewFilters;
    private Action m_onFilterChanged;

    private GUIStyle m_filterNameStyle;
    
    public FiltersPopup(List<AListViewFilter> filters, Action onFilterChanged)
    {
        m_listViewFilters = filters;
        m_onFilterChanged = onFilterChanged;

        m_filterNameStyle = new GUIStyle();
        m_filterNameStyle.padding = new RectOffset();
        m_filterNameStyle.fontSize = 10;
        m_filterNameStyle.fontStyle = FontStyle.Bold;
        m_filterNameStyle.normal.textColor = new Color(0.578f, 0.578f, 0.578f);
    }
    
    public override void OnGUI(Rect rect)
    {
        float height = 0;
        
        foreach (AListViewFilter filter in m_listViewFilters)
        {
            float totalFilterHeight = filter.GUIHeight + FILTER_NAME_HEIGHT;

            Rect elementRect = new Rect(rect);
            elementRect.y = height;
            elementRect.height = totalFilterHeight;
            GUILayout.BeginArea(elementRect, GUI.skin.label);
            GUILayout.BeginVertical("box");
            GUILayout.Label(filter.FilterName, m_filterNameStyle);
            filter.OnGUI(m_onFilterChanged);
            GUILayout.EndVertical();
            GUILayout.EndArea();
            height += totalFilterHeight;
        }

        float resetButtonWidth = 60f;
        Rect resetButtonRect = new Rect(rect);
        resetButtonRect.y = height + RESET_BUTTON_PADDING;
        resetButtonRect.height = RESET_BUTTON_HEIGHT;
        resetButtonRect.x += resetButtonRect.width - resetButtonWidth;
        resetButtonRect.width = resetButtonWidth;

        GUILayout.BeginArea(resetButtonRect, GUI.skin.label);
        if (GUILayout.Button("Reset"))
        {
        }
        GUILayout.EndArea();
    }

    public override Vector2 GetWindowSize()
    {
        Vector2 size = new Vector2(WINDOW_WIDTH, 0);
        foreach (AListViewFilter filter in m_listViewFilters)
        {
            size.y +=  filter.GUIHeight + FILTER_NAME_HEIGHT;
        }

        size.y += RESET_BUTTON_PADDING + RESET_BUTTON_HEIGHT;
        return size;
    }
}
