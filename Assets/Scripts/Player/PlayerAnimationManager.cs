using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour {

    public bool Attacking { get; set; } = false;

    [SerializeField]
    Animator m_headAnimController;

    [SerializeField]
    Animator m_bodyAnimController;

    [SerializeField]
    Animator m_legsAnimController;

    [SerializeField]
    float m_maxWalkAnimSpeed = 0.5f;

    [SerializeField]
    float m_hitTime = 2.0f;

    [SerializeField]
    Color m_hitColor = Color.red;

    SpriteRenderer[] m_renderers;

    public void AttackMelee()
    {
        m_bodyAnimController.SetTrigger("Attack");
    }

    public void AttackRanged()
    {
        m_bodyAnimController.SetTrigger("Attack");
    }

    public void Move(Vector2 move)
    {
        float animSpeed = move.magnitude * m_maxWalkAnimSpeed;

        m_legsAnimController.speed = animSpeed;
        m_headAnimController.speed = animSpeed;

        m_bodyAnimController.speed = Attacking ? 1.0f : animSpeed;

        if (animSpeed <= 0.01)
        {
            m_headAnimController.Play("Run", -1, 0.0f);
            m_legsAnimController.Play("Run", -1, 0.0f);

            if (!Attacking)
            {
                m_bodyAnimController.Play("Run", -1, 0.0f);
            }
        }

        if (move.x != 0)
        {
            SetDirection(move.x < 0);
        }
    }

    public async void Hit()
    {
        await Flash(m_hitTime).ConfigureAwait(true);
    }

    public void ReplaceController(AnimatorController controller, PlayerModifier.ModifierType type)
    {
        Animator anim = null;
        switch (type)
        {
            case PlayerModifier.ModifierType.Head:
                anim = m_headAnimController;
                break;
            case PlayerModifier.ModifierType.Body:
                anim = m_bodyAnimController;
                break;
            case PlayerModifier.ModifierType.Legs:
                anim = m_legsAnimController;
                break;
        }

        if (anim != null)
        {
            anim.runtimeAnimatorController = controller;
        }
    }

    void SetSpriteColor(Color col)
    {
        foreach (SpriteRenderer r in m_renderers)
        {
            r.color = col;
        }
    }

    async Task Flash(float duration)
    {
        float m_startTime = Time.time;

        while (Time.time - m_startTime < duration)
        {
            SetSpriteColor(m_hitColor);
            await Task.Delay(150);
            SetSpriteColor(Color.white);
            await Task.Delay(150);
        }
    }

    public void SetAnimation(PlayerModifier.ModifierType type, Animation newAnim)
    {
        /*
        switch (type)
        {
            case PlayerModifier.ModifierType.Body:
                m_bodyAnimController.anim
        }
        */
    }

    private void SetDirection(bool left)
    {
        foreach (SpriteRenderer r in m_renderers)
        {
            r.flipX = left;
        }
    }

    private void Awake()
    {
        m_renderers = GetComponentsInChildren<SpriteRenderer>();
    }
}
