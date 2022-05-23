using System.Collections;
using System.Collections.Generic;
using System.IO;
using Datastores;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ArmorGeneralView : AGeneralView
{
    private const string ARMORDATA_FOLDER_PATH = "Assets/Datastores/Editor/Impls/Armor/ArmorDatas";

    private const string UXML_PATH = "ArmorDatastore/ArmorGeneralView";
    private const string NAME_FIELD_TAG = "name-field";
    private const string PATH_LABEL_TAG = "path-label";
    private const string CREATE_BUTTON_TAG = "create-button";
    private const string FAILURE_MESSAGE_TAG = "failure-message-label";

    private TextField m_nameField;
    private Label m_pathLabel;
    private Button m_createButton;
    private Label m_failureMessageLabel;

    protected override void CreateView()
    {
        var uxmlAsset = Resources.Load<VisualTreeAsset>(UXML_PATH);
        uxmlAsset.CloneTree(this);

        m_nameField = this.Q<TextField>(NAME_FIELD_TAG);
        m_pathLabel = this.Q<Label>(PATH_LABEL_TAG);
        m_createButton = this.Q<Button>(CREATE_BUTTON_TAG);
        m_failureMessageLabel = this.Q<Label>(FAILURE_MESSAGE_TAG);

        m_nameField.RegisterValueChangedCallback(evt => { UpdatePathPreview(); });
        m_createButton.clicked += OnCreatePressed;

        UpdatePathPreview();
    }

    protected override void OnSetDatastore()
    {
    }

    protected override void ResetView()
    {
        m_failureMessageLabel.style.display = DisplayStyle.None;
    }

    private void UpdatePathPreview()
    {
        m_pathLabel.text = $"{ARMORDATA_FOLDER_PATH}/{m_nameField.value}.asset";
    }
    private void OnCreatePressed()
    {
        if (string.IsNullOrEmpty(m_nameField.value))
        {
            m_failureMessageLabel.text = "Failed to create: NAME IS EMPTY";
            m_failureMessageLabel.style.display = DisplayStyle.Flex;
            return;
        }
        
        string fullPath = $"{ARMORDATA_FOLDER_PATH}/{m_nameField.value}.asset";
        if (File.Exists(fullPath))
        {
            m_failureMessageLabel.text = "Failed to create: PATH EXISTS";
            m_failureMessageLabel.style.display = DisplayStyle.Flex;
            return;
        }

        ArmorData newData = ScriptableObject.CreateInstance<ArmorData>();
        AssetDatabase.CreateAsset(newData, fullPath);
        AssetDatabase.SaveAssets();
        m_failureMessageLabel.style.display = DisplayStyle.None;
        m_datastoreWindow.ReloadData();
        m_datastoreWindow.SelectElement(newData.Id);
    }
}
