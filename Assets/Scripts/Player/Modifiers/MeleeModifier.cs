using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerModifier;

public class MeleeModifier
{
    public virtual bool CanMove { get; } = true;
    public virtual bool ContactDamage { get; } = false;
    public virtual bool ReplacesAttack { get; } = false;

    protected Rigidbody2D m_rigidbody;
    protected PlayerAnimationManager m_animationManager;

    public MeleeModifier(Rigidbody2D rigidbody, PlayerAnimationManager animationManager)
    {
        m_rigidbody = rigidbody;
        m_animationManager = animationManager; 
    }

    public virtual void Update(bool attackPressed, bool dir)
    {
    }

    public virtual void UpdateMovement()
    {
    }

    public virtual void OnBaseAttack()
    {
    }
}
