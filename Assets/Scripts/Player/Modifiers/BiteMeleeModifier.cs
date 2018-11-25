using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteMeleeModifier : MeleeModifier
{
    bool m_direction;
    Vector2 m_lunge = new Vector2(0, 0);
    Vector2 m_lungeVelocity = new Vector2();

    public BiteMeleeModifier(Rigidbody2D rigidbody, PlayerAnimationManager animationManager) : base(rigidbody, animationManager)
    {
    }

    public override void Update(bool attackPressed, bool dir)
    {
        m_direction = dir;
    }

    public override void UpdateMovement()
    {
        m_rigidbody.velocity = m_rigidbody.velocity + m_lunge;
        m_lunge = Vector2.SmoothDamp(m_lunge, Vector2.zero, ref m_lungeVelocity, 0.1f);
    }

    public override void OnBaseAttack()
    {
        m_lunge = new Vector2(m_direction ? -10 : 10, 0);
    }
}
