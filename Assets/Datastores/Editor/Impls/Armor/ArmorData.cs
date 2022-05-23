using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorData : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private string m_id;
    [SerializeField] private Texture2D m_icon;
    [SerializeField] private string m_description;
    
    public string Id{ get { return m_id; } }

    public ArmorData()
    {
        m_id = System.Guid.NewGuid().ToString();
    }
}
