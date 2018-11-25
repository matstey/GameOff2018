using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerModifier", menuName = "PlayerModifier", order = 1)]
public class PlayerModifier : ScriptableObject
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

    [SerializeField]
    RuntimeAnimatorController m_replacementAnimator;

    public ModifierType Type { get { return m_modifierType; } }
    public PlayerStats StatModifiers { get { return m_statModifiers; } }
    public RuntimeAnimatorController ReplacementAnimator { get { return m_replacementAnimator; } }
} 
