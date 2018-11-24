using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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

    [SerializeField]
    AnimatorController m_replacementAnimator;

    public ModifierType Type { get { return m_modifierType; } }
    public PlayerStats StatModifiers { get { return m_statModifiers; } }
    public AnimatorController ReplacementAnimator { get { return m_replacementAnimator; } }
} 
