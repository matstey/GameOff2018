using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeMeleeModifier : MeleeModifier
{
    float m_buildupTime = 1.0f;
    bool m_buildup = false;
    bool m_charging = false;

    float m_buildupStart = 0;

    Vector2 m_chargeVelocity = new Vector2();
    Vector2 m_chargeLerp = new Vector2();

    public override bool CanMove { get { return !m_charging; } }
    public override bool ContactDamage { get { return m_charging; } }

    public ChargeMeleeModifier(Rigidbody2D rigidbody, PlayerAnimationManager animationManager) : base(rigidbody, animationManager)
    {
    }

    public override void Update(bool attackPressed, bool dir)
    {
        float buildupAmount = Time.time - m_buildupStart / m_buildupTime;

        if (attackPressed && !m_charging)
        {
            if (!m_buildup)
            {
                m_buildup = true;
                m_buildupStart = Time.time;
            }
            else
            {
                m_animationManager.SetChargeAmount(buildupAmount);
            }
        }
        else if (m_buildup && !m_charging)
        {
            m_buildup = false;
            m_animationManager.SetChargeAmount(0);

            if (buildupAmount >= 1)
            {
                m_charging = true;

                float chargeAmount = dir ? -15 : 15;
                m_chargeVelocity = new Vector2(chargeAmount, 0) + m_rigidbody.velocity;
            }
        }
    }

    public override void UpdateMovement()
    {
        if (m_charging)
        {
            m_chargeVelocity = Vector2.SmoothDamp(m_chargeVelocity, Vector2.zero, ref m_chargeLerp, 0.3f);

            if (m_chargeVelocity.magnitude < 0.1f)
            {
                m_charging = false;
            }
            else
            {
                m_rigidbody.velocity = m_chargeVelocity;
                m_animationManager.Move(Vector2.zero);
            }
        }
    }
}
