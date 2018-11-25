using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerModifier", menuName = "PlayerModifier", order = 1)]
public class PlayerModifier : ScriptableObject
{
    public enum BodyPart
    {
        None,
        Head,
        Body,
        Legs
    }

    public enum MeleeModifierType
    {
        None,
        Bite,
        Charge
    }

    [SerializeField]
    BodyPart m_modifierType;

    [SerializeField]
    PlayerStats m_statModifiers;

    [SerializeField]
    RuntimeAnimatorController m_replacementAnimator;

    [SerializeField]
    MeleeModifierType m_meleeModifierType = PlayerModifier.MeleeModifierType.None;

    public MeleeModifierType MeleeModifier { get { return m_meleeModifierType; } }
    public BodyPart Type { get { return m_modifierType; } }
    public PlayerStats StatModifiers { get { return m_statModifiers; } }
    public RuntimeAnimatorController ReplacementAnimator { get { return m_replacementAnimator; } }
} 
