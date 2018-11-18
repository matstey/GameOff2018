using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModifier : MonoBehaviour
{
    public enum ModifierType
    {
        Head,
        Body,
        Legs, 
        Accessory
    }

    [SerializeField]
    ModifierType m_modifierType;

    [SerializeField]
    PlayerStats m_statModifiers;

    public ModifierType Type { get { return m_modifierType; } }
    public PlayerStats StatModifiers { get { return m_statModifiers; } }

    public void OnAdded()
    {

    }
}
